//-------------------------------------------------------------------------------
// <copyright file="Program.cs" company="bbv Software Services AG">
//   Copyright (c) 2010 bbv Software Services AG
// </copyright>
//-------------------------------------------------------------------------------

namespace Winkeladvokat
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Main entry point of the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ////Application.Run(view);
        }
    }
}
