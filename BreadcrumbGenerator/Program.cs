using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BreadcrumbGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            BreadcrumbGenerator("Http://thisisatestcom/pages/doc/index.html", '/');
        }

        public static string BreadcrumbGenerator(string URL, char splitChar)
        {
            /************************************************************************/
            /*Name: Breadcrumb Generator
            /*Description: given a valid URL and the character you want each breadcrumb split by, will generate the breadcrumbs at the top of a web page
            /* Inputs: A valid URL, the character you would like each breadcrumb split by
            /* outputs: a valid breadcrumb at the top of a page
            /* factors in a lot of error detection
            /*******************************************************************************/

            string[] splitURL = URL.Split("/");
            string breadcrumb = "";
            int numelements = splitURL.Length;
            int startpoint = 1;
            string URLLink = "";
            string wordsinlink = "";

            /***************************SORT OUT THE BEGINNING OF THE STRING REMOVING UN-NECESSARY CHARS*******/
            if (splitURL[0].Contains("http") || splitURL[0].Contains("https"))
            {
                //this gets rid of the  http://
                startpoint = startpoint + 2;
            }
            /***************************SORT OUT THE END OF THE STRING REMOVING UN-NECESSARY CHARS*******/

            var pattern = @"[.?#]";
            var match = Regex.Match(splitURL[numelements - 1], pattern);

            //if there is a .html/extension then get rid of it
            if (Regex.IsMatch(splitURL[numelements - 1], pattern))
            {
                splitURL[numelements - 1] = splitURL[numelements - 1].Substring(0, match.Index);
            }
            //check if the last instance contains index
            if (splitURL[splitURL.Length - 1].Contains("index") || splitURL[splitURL.Length - 1] == "")
            {
                numelements = numelements - 1;
            }

            //if there is only one element, then there is only one active link
            if (numelements - startpoint == 0)
            {
                breadcrumb = "<span class=\"active\">HOME</span>";
            }
            else
            {

                breadcrumb = "<a href=\"/\">HOME</a>" + splitChar;
            }


            //sort out layout for rest of the breadbcrumb
            for (int count = startpoint; count < numelements; count++)
            {
                URLLink = URLLink + "/" + splitURL[count];

                wordsinlink = "";

                if (splitURL[count].Length > 30)
                {
                    //split it by hyphens
                    var HyphenSplit = splitURL[count].Split("-");
                    string[] excludedWords = { "the", "of", "in", "from", "by", "with", "and", "or", "for", "to", "at", "a" };
                    //capitalise each letter
                    for (int HyphenCount = 0; HyphenCount < HyphenSplit.Length; HyphenCount++)
                    {
                        if (!excludedWords.Contains(HyphenSplit[HyphenCount]))
                        {
                            wordsinlink = wordsinlink + HyphenSplit[HyphenCount].ToUpper()[0];
                        }
                    }
                }
                else
                {
                    wordsinlink = splitURL[count].ToUpper();

                    if (wordsinlink.Contains("-"))
                    {
                        wordsinlink = wordsinlink.Replace("-", " ");
                    }
                }
                //means its the end of the URL, so treat it differently.
                if (count == numelements - 1)
                {
                    //add the active element to the end
                    breadcrumb = breadcrumb + "<span class=\"active\">" + wordsinlink + "</span>";
                }
                else
                {
                    breadcrumb = breadcrumb + "<a href=\"" + URLLink + "/\">" + wordsinlink + "</a>" + splitChar;
                }
            }
            return breadcrumb;
        }

    }
}
