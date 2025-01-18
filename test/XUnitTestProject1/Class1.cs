using System;

namespace XUnitTestProject1;

public class Class1
{
    public static string EncryptRC4wq(string ckey, string str)
    {
        int[] s = new int[256];
        for (int i = 0; i < 256; i++)
        {
            s[i] = i;
        }
        //密钥转数组
        char[] keys = ckey.ToCharArray();//密钥转字符数组
        int[] key = new int[keys.Length];
        for (int i = 0; i < keys.Length; i++)
        {
            key[i] = keys[i];
        }
        //明文转数组
        char[] datas = str.ToCharArray();
        int[] mingwen = new int[datas.Length];
        for (int i = 0; i < datas.Length; i++)
        {
            mingwen[i] = datas[i];
        }

        //通过循环得到 256 位的数组(密钥)
        int j = 0;
        int key_len = key.Length;
        for (int i = 0; i < 256; i++)
        {
            j = (j + s[i] + key[i % key_len]) % 256;
            //swap
            int temp = s[i];
            s[i] = s[j];
            s[j] = temp;
        }
        //根据上面的 256 的密钥数组 和 明文得到密文数组
        int x = 0, y = 0, c;
        int data_len = mingwen.Length;
        int[] miwen = new int[data_len];
        string ret = "";
        for (int i = 0; i < data_len; i++)
        {
            x = (x + 1) % 256;
            y = (y + s[x]) % 256;

            //swap
            int temp = s[x];
            s[x] = s[y];
            s[y] = temp;

            c = s[x] + s[y];
            c = c % 256;
            miwen[i] = mingwen[i] ^ s[c];
            ret += (char)miwen[i];
        }

        return ret;
    }
}
