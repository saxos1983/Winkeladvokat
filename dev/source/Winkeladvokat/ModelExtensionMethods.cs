namespace Winkeladvokat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ModelExtensionMethods
    {
         public static Field SelectByPosition(this IEnumerable<Field> collection, int row, int column)
         {
             return collection.Single(f => f.Row == row && f.Column == column);
         }

         public static string GetColorName(this System.Windows.Media.Color color)
         {
             Type colors = typeof(System.Windows.Media.Colors);
             foreach (var propertyInfo in colors.GetProperties())
             {
                 if (((System.Windows.Media.Color)propertyInfo.GetValue(null, null)) == color)
                 {
                     return propertyInfo.Name;
                 }
             }

             return color.ToString();
         }
    }
}