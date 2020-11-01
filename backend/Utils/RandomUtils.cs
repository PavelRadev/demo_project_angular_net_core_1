﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public static class RandomUtils
    {
        public static int GetRandomNumber(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max);
        }

        public static string GetRandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
}
