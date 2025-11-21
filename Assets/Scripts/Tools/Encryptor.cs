using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class Encryptor
{
    // Настройки
    private const int SaltSize = 16; // байт
    private const int KeySizeBits = 256; // 128/192/256
    private const int Iterations = 100_000; // количество итераций PBKDF2

    public static string EncryptToBase64(string plainText, string password)
    {
        if (plainText == null) throw new ArgumentNullException(nameof(plainText));
        if (password == null) throw new ArgumentNullException(nameof(password));

        using (var aes = Aes.Create())
        {
            aes.KeySize = KeySizeBits;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Соль и IV - случайные
            byte[] salt = RandomBytes(SaltSize);
            aes.GenerateIV();
            byte[] iv = aes.IV;

            // Извлекаем ключ нужной длины из пароля+соли
            using (var kdf = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                aes.Key = kdf.GetBytes(aes.KeySize / 8); // гарантированно 16/24/32 байта
            }

            byte[] cipherBytes;
            using (var ms = new MemoryStream())
            using (var crypto = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (var writer = new StreamWriter(crypto, Encoding.UTF8))
            {
                writer.Write(plainText);
                writer.Flush();
                crypto.FlushFinalBlock();
                cipherBytes = ms.ToArray();
            }

            // Формат хранения: salt + iv + ciphertext
            byte[] result = new byte[salt.Length + iv.Length + cipherBytes.Length];
            Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
            Buffer.BlockCopy(iv, 0, result, salt.Length, iv.Length);
            Buffer.BlockCopy(cipherBytes, 0, result, salt.Length + iv.Length, cipherBytes.Length);

            return Convert.ToBase64String(result);
        }
    }

    public static string DecryptFromBase64(string base64Input, string password)
    {
        if (base64Input == null) throw new ArgumentNullException(nameof(base64Input));
        if (password == null) throw new ArgumentNullException(nameof(password));

        // Нормализация base64: убрать пробелы/переносы, восстановить padding, заменить URL-safe символы
        base64Input = base64Input.Trim().Replace("\r", "")
                                 .Replace("\n", "")
                                 .Replace('-', '+')
                                 .Replace('_', '/');
        // Восстановление паддинга
        switch (base64Input.Length % 4)
        {
            case 2: base64Input += "=="; break;
            case 3: base64Input += "="; break;
        }

        byte[] allBytes;
        try
        {
            allBytes = Convert.FromBase64String(base64Input);
        }
        catch (FormatException ex)
        {
            throw new FormatException("Входная строка не является корректным Base64: " + ex.Message, ex);
        }

        if (allBytes.Length < SaltSize + 16) // минимум: salt + iv
            throw new ArgumentException("Входные данные слишком короткие.");

        byte[] salt = new byte[SaltSize];
        Buffer.BlockCopy(allBytes, 0, salt, 0, SaltSize);

        // AES block size для IV обычно 128 бит => 16 байт
        int ivSize = 16;
        byte[] iv = new byte[ivSize];
        Buffer.BlockCopy(allBytes, SaltSize, iv, 0, ivSize);

        int cipherOffset = SaltSize + ivSize;
        int cipherLength = allBytes.Length - cipherOffset;
        byte[] cipherBytes = new byte[cipherLength];
        Buffer.BlockCopy(allBytes, cipherOffset, cipherBytes, 0, cipherLength);

        using (var aes = Aes.Create())
        {
            aes.KeySize = KeySizeBits;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.IV = iv;

            using (var kdf = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                aes.Key = kdf.GetBytes(aes.KeySize / 8);
            }

            using (var ms = new MemoryStream(cipherBytes))
            using (var crypto = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
            using (var reader = new StreamReader(crypto, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }

    private static byte[] RandomBytes(int length)
    {
        byte[] b = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
            rng.GetBytes(b);
        return b;
    }
}
