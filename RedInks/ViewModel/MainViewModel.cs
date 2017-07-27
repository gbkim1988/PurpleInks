using GalaSoft.MvvmLight;
using System.Windows;
using System.IO;
using RedInks.Interfaces;
using System;
using System.Collections.Generic;
using RedInks.Utils;
using System.Threading.Tasks;
using RedInks.Models;

namespace RedInks.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public List<Diagnosis> TestBinding { get; set; }
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                
            }
            else
            {
                // Code runs "for real"
                //MessageBox.Show(Directory.GetCurrentDirectory());
                // Where Am I?
                //MessageBox.Show(String.Join("-",service.GetCommandLineArguments()));
                /*
                Task<bool> result = new ExecuteProcess().Execute(
                "outlookattachview64.exe",
                "D:\\powershell-project\\powershell-repo\\MALSCAN\\module\\bin\\Outlook",
                new System.Collections.Generic.List<String> { "/sxml", "C:\\gura.xml" });
                */
                //TestBinding = result.IsCompleted;
                TestBinding = new List<Diagnosis>();
                TestBinding.Add(new Diagnosis(new MA001()));
                TestBinding.Add(new Diagnosis(new MA001()));
                TestBinding.Add(new Diagnosis(new MA001()));
                TestBinding.Add(new Diagnosis(new MA001()));
                TestBinding.Add(new Diagnosis(new MA001()));
                TestBinding.Add(new Diagnosis(new MA001()));
            }
        }

        private void ArgumentParser(IEnumerable<String> args) {

        }
    }
}