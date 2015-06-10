using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OurHelper
{
    public class DataHelper
    {
        public static List<T> FillList<T>(DataTable dataTable)
        {
            //实例化一个List<>泛型集合
            List<T> DataList = new List<T>();
            foreach (DataRow t_row in dataTable.Rows)
            {
                T RowInstance = Activator.CreateInstance<T>();//动态创建数据实体对象 
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    PropertyInfo Property = typeof(T).GetProperty(dataTable.Columns[i].ColumnName);
                    try
                    {
                        if (IsSetValue(t_row[i]))
                        {
                            Property.SetValue(RowInstance, HackType(t_row[i], Property.PropertyType), null);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                DataList.Add(RowInstance);
            }
            return DataList;
        }
        public static bool IsSetValue(object obj)
        {
            return !(obj is DBNull);
        }
        private static object HackType(object value, Type conversionType)
        {
            return value;
        }
    }
}
