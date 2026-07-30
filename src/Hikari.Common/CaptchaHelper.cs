using System.Security.Cryptography;
using SkiaSharp;

namespace Hikari.Common;
/// <summary>
/// 验证码帮助类
/// </summary>
public class CaptchaHelper
{
    /// <summary>
    /// 创建由 5 位随机字母和数字组成的图形验证码。
    /// </summary>
    /// <remarks>
    /// 字符集排除了易混淆字符（<c>0</c>、<c>O</c>、<c>I</c>、<c>1</c>），
    /// 并使用加密安全的随机数生成器，防止验证码被预测。
    /// </remarks>
    /// <returns>随机验证码字符串和对应 PNG 图片的 Base64 编码。</returns>
    public static (string randomCode, string pic) CreateRandomCaptcha()
    {
        const string captchaCharacters = "23456789ABCDEFGHJKLMNPQRSTUVWXYZ";
        const int captchaLength = 5;

        // 使用加密安全随机数生成器逐位生成验证码，避免 Random 的可预测性。
        char[] captchaCodeCharacters = new char[captchaLength];
        for (int index = 0; index < captchaCodeCharacters.Length; index++)
        {
            int characterIndex = RandomNumberGenerator.GetInt32(captchaCharacters.Length);
            captchaCodeCharacters[index] = captchaCharacters[characterIndex];
        }

        string randomCode = new string(captchaCodeCharacters);
        byte[] imageBytes = CreateCaptchaImage(randomCode);
        string pic = Convert.ToBase64String(imageBytes);
        return (randomCode, pic);
    }

    /// <summary>
    /// 创建算式验证码
    /// </summary>
    /// <returns>随机算式，算式结果，图片base64</returns>
    public static (string randomCode, int value, string pic) CreateArithmeticCaptcha()
    {
        var captchaCode = GetCaptchaCode();
        byte[] bytes = CreateCaptchaImage(captchaCode.randomCode);
        string pic = Convert.ToBase64String(bytes);
        return (captchaCode.randomCode, captchaCode.value, pic);
    }

    /// <summary>
    /// 获得表达式和表达式结果
    /// </summary>
    /// <returns>随机表达式，表达式结果</returns>
    private static (string randomCode, int value) GetCaptchaCode()
    {
        int value = 0;
        char[] operators = { '+', '-', '*' };
        string randomCode = string.Empty;
        Random random = new Random();

        int first = random.Next() % 10;
        int second = random.Next() % 10;
        char operatorChar = operators[random.Next(0, operators.Length)];
        switch (operatorChar)
        {
            case '+': value = first + second; break;
            case '-':
                // 第1个数要大于第二个数
                if (first < second)
                {
                    (first, second) = (second, first);
                }
                value = first - second;
                break;
            case '*': value = first * second; break;
        }

        char code = (char)('0' + (char)first);
        randomCode += code;
        randomCode += operatorChar;
        code = (char)('0' + (char)second);
        randomCode += code;
        randomCode += "=?";
        return (randomCode, value);
    }
    /// <summary>
    /// 生成验证码图片
    /// </summary>
    /// <param name="randomCode">随机码</param>
    /// <returns>图片</returns>
    private static byte[] CreateCaptchaImage(string randomCode)
    {
        const int CharacterSpacing = 29;
        const int HorizontalPadding = 12;
        const int ImageHeight = 48;
        const int MaximumRotationDegrees = 20;
        const float FontSize = 27;
        const float OuterOutlineWidth = 5.4F;
        const float CharacterStrokeWidth = 2.5F;
        const int NoisePointCount = 35;
        int imageWidth = (randomCode.Length * CharacterSpacing) + (HorizontalPadding * 2);

        using SKBitmap bitmap = new SKBitmap(imageWidth, ImageHeight);
        using SKCanvas canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);

        // 此随机数仅用于控制图像外观，不参与验证码文本及其校验。
        Random random = Random.Shared;
        DrawBackgroundNoise(canvas, bitmap, random, NoisePointCount);

        SKColor[] characterColors =
        {
            new SKColor(255, 139, 0),
            new SKColor(0, 152, 210),
            new SKColor(27, 174, 89),
            new SKColor(234, 76, 95),
            new SKColor(156, 77, 191),
        };
        string[] fontFamilies =
        {
            "Comic Sans MS",
            "Arial Rounded MT Bold",
            "Verdana",
            "Microsoft Sans Serif",
        };

        // 每个字符独立设置字体、颜色、旋转角度与垂直偏移，形成示例中的活泼手写风格。
        for (int index = 0; index < randomCode.Length; index++)
        {
            float characterX = HorizontalPadding + (CharacterSpacing * index) + (CharacterSpacing / 2F);
            float characterY = 32 + random.Next(-3, 4);
            float rotationDegrees = random.Next(-MaximumRotationDegrees, MaximumRotationDegrees + 1);
            SKColor characterColor = characterColors[random.Next(characterColors.Length)];
            string fontFamily = fontFamilies[random.Next(fontFamilies.Length)];

            using SKTypeface typeface = SKTypeface.FromFamilyName(fontFamily, SKFontStyle.Bold);
            using SKFont font = new SKFont(typeface, FontSize);
            using SKPaint outlinePaint = new SKPaint
            {
                Color = SKColors.White,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = OuterOutlineWidth,
                StrokeJoin = SKStrokeJoin.Round,
            };
            using SKPaint characterPaint = new SKPaint
            {
                Color = characterColor,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = CharacterStrokeWidth,
                StrokeJoin = SKStrokeJoin.Round,
            };

            // 先以白色粗描边分离相邻字符，再叠加彩色描边；不填充字形内部，实现粗体空心效果。
            canvas.Save();
            canvas.Translate(characterX, characterY);
            canvas.RotateDegrees(rotationDegrees);
            canvas.DrawText(randomCode[index].ToString(), 0, 0, SKTextAlign.Center, font, outlinePaint);
            canvas.DrawText(randomCode[index].ToString(), 0, 0, SKTextAlign.Center, font, characterPaint);
            canvas.Restore();
        }

        using SKData encodedData = bitmap.Encode(SKEncodedImageFormat.Png, 100);
        return encodedData.ToArray();
    }

    /// <summary>
    /// 在白色背景上绘制低对比度噪点和干扰线，增加机器识别难度且不影响人工阅读。
    /// </summary>
    /// <param name="canvas">用于绘制验证码的画布。</param>
    /// <param name="bitmap">用于确定绘制范围的位图。</param>
    /// <param name="random">仅用于生成视觉样式的随机数生成器。</param>
    /// <param name="noisePointCount">需要绘制的背景噪点数量。</param>
    private static void DrawBackgroundNoise(SKCanvas canvas, SKBitmap bitmap, Random random, int noisePointCount)
    {
        using SKPaint noisePaint = new SKPaint
        {
            Color = new SKColor(210, 220, 230),
            IsAntialias = true,
            StrokeWidth = 1,
        };
        for (int index = 0; index < noisePointCount; index++)
        {
            canvas.DrawCircle(random.Next(bitmap.Width), random.Next(bitmap.Height), 1, noisePaint);
        }

        // 使用半透明曲线提供视觉干扰，避免深色直线遮挡验证码主体。
        using SKPaint linePaint = new SKPaint
        {
            Color = new SKColor(170, 190, 210, 120),
            IsAntialias = true,
            StrokeWidth = 1.2F,
            Style = SKPaintStyle.Stroke,
        };
        for (int index = 0; index < 2; index++)
        {
            SKPoint[] points =
            {
                new SKPoint(0, random.Next(bitmap.Height)),
                new SKPoint(bitmap.Width / 3F, random.Next(bitmap.Height)),
                new SKPoint((bitmap.Width * 2F) / 3F, random.Next(bitmap.Height)),
                new SKPoint(bitmap.Width, random.Next(bitmap.Height)),
            };
            canvas.DrawPoints(SKPointMode.Polygon, points, linePaint);
        }
    }
}
