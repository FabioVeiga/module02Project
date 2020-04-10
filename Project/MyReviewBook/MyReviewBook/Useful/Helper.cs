using MyReviewBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyReviewBook.Useful
{
    public class Helper
    {
        public static string formatDate(string input)
        {
            DateTime date = DateTime.Parse(input);
            return date.ToString("yyyy-MM-dd");
        }

        public static DateTime ajustDate(string input)
        {
            DateTime date;
            try
            {
                date = DateTime.Parse(input);
                if (input.Length <= 4)
                {
                    date = new DateTime(int.Parse(input), 01, 01);
                }
                else
                {
                    date = DateTime.Parse(input);
                }
            }
            catch
            {
                date = new DateTime();
                return date;
            }
            return date;
        }

        public static List<DashboardModel> eliminateDuplicates(List<DashboardModel> listBookModels)
        {
            //Compare if the list has duplicates
            int aux = 0;
            int count = 1;
            string titleAjustOne = "";
            string titleAjustTwo = "";
            do
            {
                titleAjustOne = listBookModels[aux].Book.NameBook.ToLower().Replace(" ", "");
                if (count == listBookModels.Count)
                {
                    break;
                }
                titleAjustTwo = listBookModels[count].Book.NameBook.ToLower().Replace(" ", "");
                if (titleAjustOne == titleAjustTwo)
                {
                    listBookModels.RemoveAt(count);
                }
                else
                {
                    aux++;
                    count++;
                }
            } while (aux < listBookModels.Count);
            return listBookModels;
        }
    }
}
