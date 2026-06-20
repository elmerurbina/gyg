using System.Globalization;
using System.Threading;
using GyG.Presentacion;

namespace GyG;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Set the culture to Nicaraguan Spanish
        CultureInfo nicaraguaCulture = new CultureInfo("es-NI");
        
        // Configure currency settings for Nicaraguan Córdoba (NIO)
        nicaraguaCulture.NumberFormat.CurrencySymbol = "C$";
        nicaraguaCulture.NumberFormat.CurrencyPositivePattern = 0;  // C$1.23
        nicaraguaCulture.NumberFormat.CurrencyNegativePattern = 0;   // (C$1.23) for negative
        nicaraguaCulture.NumberFormat.CurrencyDecimalDigits = 2;
        nicaraguaCulture.NumberFormat.CurrencyDecimalSeparator = ".";
        nicaraguaCulture.NumberFormat.CurrencyGroupSeparator = ",";
        
        // Apply the culture to the current thread
        Thread.CurrentThread.CurrentCulture = nicaraguaCulture;
        Thread.CurrentThread.CurrentUICulture = nicaraguaCulture;
        
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new LoginForm());
    }
}