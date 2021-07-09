using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace Triangles
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /*
         * When a side length textbox is changed in the window, update its classifications 
         */
        private void updateTriangle(object sender, TextChangedEventArgs e)
        {
            String a = side1.Text;
            String b = side2.Text;
            String c = side3.Text;

            resultText.Content = triangleDeterminer(a, b, c);
        }

        /*
         * This method is designed to, given all of the side lengths as strings, determine what classifications the triangle 
         * would have and return that information in a readable fashion
         * 
         * String a - the first side length
         * String b - the second side length
         * String c - the third side length
         */
        private String triangleDeterminer(String a, String b, String c)
        {
            String cookieCutter = "These side lengths ";
            // Make sure all of the text boxes aren't empty, and continue if that is true
            // Otherwise, return an empty string
            if (!(String.IsNullOrEmpty(a) || String.IsNullOrEmpty(b) || String.IsNullOrEmpty(c)))
            {
                double aVal, bVal, cVal;

                // Attempt to read the text values and store their values in the appropriate integer
                // If that fails, then one of the textboxes isn't a number, and is thus invalid
                if (double.TryParse(a, out aVal) && double.TryParse(b, out bVal) && double.TryParse(c, out cVal))
                {
                    // Make sure the values can produce a triangle
                    // Continue if they can, or return that they can't
                    if ((aVal + bVal <= cVal) || (aVal + cVal <= bVal) || (bVal + cVal <= aVal))
                    {
                        return cookieCutter + "cannot make a triangle";
                    }

                    // Since the triangle is valid, we need to know the angles of the triangle
                    double angle1, angle2, angle3;
                    angle1 = lawOfCosines(aVal, bVal, cVal);
                    angle2 = lawOfCosines(aVal, cVal, bVal);
                    angle3 = lawOfCosines(cVal, bVal, aVal);

                    // Determine the type of triangle between: acute, right, and obtuse (angle)
                    String angleType;
                    double maxAngle = Math.Max(Math.Max(angle1, angle2), angle3);
                    if (maxAngle < 90.00)
                    {
                        angleType = "acute";
                    }
                    else if (maxAngle == 90.00)
                    {
                        angleType = "right";
                    }
                    else
                    {
                        angleType = "obtuse";
                    }

                    // Determine the type of triangle between: equilateral, isosceles, and scalene (side lengths)
                    String triangleType;
                    if (aVal == bVal && bVal == cVal)
                    {
                        triangleType = "equilateral";
                    }
                    else if (aVal == bVal || bVal == cVal || aVal == cVal)
                    {
                        triangleType = "isosceles";
                    }
                    else
                    {
                        triangleType = "scalene";
                    }

                    // Create a string that contains all of the degrees in a readable format 
                    String angleString;
                    angleString = angle1.ToString("00.00") + " degrees, " + angle2.ToString("00.00") + " degrees, and " +
                        angle3.ToString("00.00") + " degrees";

                    // Return the entire triangle classification with the degrees included
                    return cookieCutter + "make a " + angleType + " " + triangleType + " triangle with angles of " + angleString;
                }
            }
            return "";
        }

        /* 
         * Use the law of cosines (c2 = a2 + b2 - 2ab cos(angle)) to determine the angles and thus
         * what classifications it will get
         * 
         * double A - Side A of a triangle
         * double B - Side B of a triangle
         * double C - Side C of a triangle
         */
        private double lawOfCosines(double A, double B, double C)
        {
            double step1 = C*C - A*A - B*B;
            double step2 = step1 / (-2*A*B);
            return Math.Acos(step2) * 180 / Math.PI;
        }

        /*
         * Determine if a string contains only numbers (1 decimal allowed)
         * String text - The input text
         */
        private bool NumbersOnlyMatch(String text)
        {
            Regex allowed = new Regex("^\\d*\\.?\\d*$");
            return allowed.IsMatch(text);
        }

        /*
         * Only allow a side length textbox to contain numbers (1 decimal allowed)
         */
        private void NumbersOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumbersOnlyMatch(((TextBox)sender).Text + e.Text);
        }

        /*
         * Only allow pasting to a side length textbox if it would create a number (1 decimal allowed)
         */
        private void NumbersOnly(object sender, DataObjectPastingEventArgs e)
        {
            if (!NumbersOnlyMatch(((TextBox)sender).Text + e.SourceDataObject.GetData(DataFormats.UnicodeText) as string))
            {
                e.CancelCommand();
            }
        }
    }
}
