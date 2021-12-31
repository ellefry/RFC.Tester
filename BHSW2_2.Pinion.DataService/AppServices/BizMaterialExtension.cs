using System;

namespace BHSW2_2.Pinion.DataService.AppServices
{
    public static class BizMaterialExtension
    {
        public static string ToSapMaterial(this string material, int targetLength = 10, string format = "4-3-3")
        {
            if(string.IsNullOrEmpty(material))
                throw new ArgumentNullException(nameof(material));

            material = material.Trim();
            if (material.Length == targetLength)
            {
                return ConvertByFormat(material, format);
            }
            if (material.Length == targetLength + 1)
            {
                return ConvertByFormat(material, "4-4-3");
            }
            return material;
        }

        private static string ConvertByFormat(string value, string format)
        {
            if (string.IsNullOrEmpty(format))
                throw new ArgumentNullException(nameof(format));

            var formation = format.Split('-');
            var f0 = int.Parse(formation[0]);
            var f1 = int.Parse(formation[1]);
            var f2 = int.Parse(formation[2]);

            var t1 = value[..f0];
            var t2 = value[f0..(f0 + f1)];
            var t3 = value[^f2..^0];

            value = $"{value[..f0]} {value[f0..(f0 + f1)]} {value[^f2..^0]}";
            return value;

        }
    }
}
