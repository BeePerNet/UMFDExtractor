using UMFDExtractor.ViewModels;
using ReactiveUI;
using System;

namespace UMFDExtractor.Windows
{
    /// <summary>
    /// Logique d'interaction pour ValuesWindow.xaml
    /// </summary>
    [Obsolete]
    public partial class KSPValuesWindow : ReactiveWindow<KSPValuesWindowViewModel>
    {
        public KSPValuesWindow()
        {
            InitializeComponent();
        }
    }
}
