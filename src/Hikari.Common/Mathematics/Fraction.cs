namespace Hikari.Common.Mathematics;
/// <summary>
/// 分数
/// </summary>
public class Fraction
{
    private long _numerator; // 分子
    /// <summary>
    /// 分子
    /// </summary>
    public long Numerator
    {
        get
        {
            return _numerator;
        }
        set
        {
            _numerator = value;
        }
    }
    private long _denominator; // 分母
    /// <summary>
    /// 分母
    /// </summary>
    public long Denominator
    {

        get
        {
            return _denominator;
        }
        set
        {
            if (value == 0)
            {
                throw new InvalidOperationException("分母不能为0");
            }
            _denominator = value;
        }

    }
    /// <summary>
    /// 创建分数
    /// </summary>
    public Fraction() { }
    /// <summary>
    /// 创建分数(默认分母为1)
    /// </summary>
    /// <param name="numerator">分子</param>
    public Fraction(long numerator)
    {
        _numerator = numerator;
        _denominator = 1;
    }
    /// <summary>
    /// 创建分数(默认分母为1)
    /// </summary>
    /// <param name="numerator">分子</param>
    public Fraction(double numerator)
    {
        if (numerator != 0)
        {
            string str;
            // 检查是否为有限小数
            if (IsFiniteDecimal(numerator))
            {
                str = FiniteDecimalToFraction(numerator);
            }
            else
            {
                str = InfiniteDecimalToFraction(numerator);
            }
            var fra = new Fraction(str);
            _numerator = fra.Numerator;
            _denominator = fra.Denominator;
        }


    }
    /// <summary>
    /// 创建分数
    /// </summary>
    /// <param name="numerator">分子</param>
    /// <param name="denominator">分母</param>
    public Fraction(int numerator, int denominator)
    {
        _numerator = numerator;
        _denominator = denominator;
    }
    /// <summary>
    /// 创建分数
    /// </summary>
    /// <param name="fraction">分数</param>
    public Fraction(string fraction)
    {
        if (fraction.Contains("/"))
        {
            var parts = fraction.Split('/');
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int numerator) &&
                int.TryParse(parts[1], out int denominator))
            {
                if (denominator == 0)
                {
                    throw new InvalidOperationException("分母不能为0");
                }

                _numerator = numerator;
                _denominator = denominator;
            }
            else
            {
                throw new InvalidOperationException("不是有效的分数格式");
            }
        }
    }

    /// <summary>
    /// 约分
    /// </summary>
    public void Reduction()
    {
        var gcd = MathHelper.GetGreatestCommonDivisor3(_numerator, _denominator);
        _numerator /= gcd;
        _denominator /= gcd;
    }
    /// <summary>
    /// 倒数
    /// </summary>
    /// <returns></returns>
    public Fraction Exchange()
    {
        Fraction NewFra = new Fraction();

        NewFra.Denominator = this._numerator;
        NewFra.Numerator = this._denominator;
        return NewFra;
    }
    /// <summary>
    /// 加法
    /// </summary>
    /// <param name="fra"></param>
    /// <returns></returns>
    public Fraction Add(Fraction fra)
    {
        //存放加法结果
        Fraction sum = new Fraction
        {
            Numerator = this._numerator * fra.Denominator + fra.Numerator * this._denominator,
            Denominator = this._denominator * fra.Denominator
        };
        //约分
        sum.Reduction();

        return sum;
    }
    /// <summary>
    /// 减法
    /// </summary>
    /// <param name="subtrahend">减数</param>
    /// <returns></returns>
    public Fraction Sub(Fraction subtrahend)
    {
        //存放加法结果
        Fraction result = new Fraction();
        result.Numerator = this._numerator * subtrahend.Denominator - subtrahend.Numerator * this._denominator;
        result.Denominator = this._denominator * subtrahend.Denominator;
        //约分
        result.Reduction();

        return result;
    }
    /// <summary>
    /// 乘法
    /// </summary>
    /// <param name="f1"></param>
    /// <returns></returns>
    public Fraction Multiple(Fraction f1)
    {
        Fraction result = new Fraction
        {
            Denominator = this._denominator * f1.Denominator,
            Numerator = this._numerator * f1.Numerator
        };
        //约分
        result.Reduction();
        return result;
    }
    /// <summary>
    /// 除法
    /// </summary>
    /// <param name="divisor">除数</param>
    /// <returns></returns>
    public Fraction Divided(Fraction divisor)
    {

        Fraction result = new Fraction();
        //求倒数
        Fraction newfra = divisor.Exchange();

        result.Numerator = this._numerator * newfra.Numerator;
        result.Denominator = this._numerator * newfra.Denominator;
        //约分
        result.Reduction();

        return result;
    }
    /// <summary>
    /// 将分数转换为小数
    /// </summary>
    /// <returns></returns>
    public double ToDouble()
    {
        return _numerator * 1.0 / _denominator;
    }
    /// <summary>
    /// 将分数转换为字符串
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{_numerator}/{_denominator}";
    }
    /// <summary>
    /// 检查是否是有限小数，假设最多15位小数为有限小数
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private bool IsFiniteDecimal(double value)
    {
        string valueStr = value + "";
        int decimalIndex = valueStr.IndexOf('.');
        if (decimalIndex == -1)
        {
            return true;
        }
        return valueStr.Substring(decimalIndex + 1).Length <= 15; // 假设最多15位小数为有限小数
    }
    /// <summary>
    /// 有限小数转换为分数
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private string FiniteDecimalToFraction(double value)
    {
        int decimalPlaces = GetDecimalPlaces(value);
        long numerator = (long)(value * System.Math.Pow(10, decimalPlaces));
        long denominator = (long)System.Math.Pow(10, decimalPlaces);

        // 化简分数
        long gcd = MathHelper.GetGreatestCommonDivisor3(numerator, denominator);
        numerator /= gcd;
        denominator /= gcd;

        // 返回分数形式
        if (denominator == 1)
        {
            return numerator.ToString();
        }
        else
        {
            return $"{numerator}/{denominator}";
        }
    }
    /// <summary>
    /// 使用连分数算法将无限小数转换为分数
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private string InfiniteDecimalToFraction(double value)
    {
        int maxDenominator = 10 ^ 15; // 设置最大分母限制
        int wholePart = (int)value;
        double fractionalPart = value - wholePart;

        int numerator = 0;
        int denominator = 1;

        while (System.Math.Abs(fractionalPart * denominator - numerator) > 1e-10 && denominator <= maxDenominator)
        {
            int newNumerator = (int)(fractionalPart * denominator);
            int newDenominator = denominator;
            numerator = newNumerator;
            denominator = newDenominator + 1;
        }

        if (denominator > maxDenominator)
        {
            // 如果超过最大分母限制，返回最接近的分数
            double bestError = double.MaxValue;
            int bestNumerator = 0;
            int bestDenominator = 1;

            for (int d = 1; d <= maxDenominator; d++)
            {
                int n = (int)(value * d);
                double error = System.Math.Abs(value - (double)n / d);
                if (error < bestError)
                {
                    bestError = error;
                    bestNumerator = n;
                    bestDenominator = d;
                }
            }

            numerator = bestNumerator;
            denominator = bestDenominator;
        }

        // 返回纯分数形式
        return $"{numerator}/{denominator}";
    }
    private static int GetDecimalPlaces(double value)
    {
        string valueStr = value.ToString("G17"); // 使用高精度格式
        int decimalIndex = valueStr.IndexOf('.');
        if (decimalIndex == -1)
        {
            return 0;
        }
        return valueStr.Length - decimalIndex - 1;
    }
}