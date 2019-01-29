/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Inspur.Billing"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Inspur.Billing.ViewModel.Issue;
using Inspur.Billing.ViewModel.Login;
using Inspur.Billing.ViewModel.Setting;
using Inspur.Billing.ViewModel.Statistics;

namespace Inspur.Billing.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<CreditViewModel>();
            SimpleIoc.Default.Register<PrintViewModel>();
            SimpleIoc.Default.Register<PinViewModel>();
            SimpleIoc.Default.Register<TaxPayerSettingVm>();
            SimpleIoc.Default.Register<ParameterSettingVm>();
            SimpleIoc.Default.Register<SoftwareSettingVm>();
            SimpleIoc.Default.Register<SystemTestVm>();
            SimpleIoc.Default.Register<XZPeriodicVm>();
            SimpleIoc.Default.Register<PrintSettingVm>();
            SimpleIoc.Default.Register<OperationModeVm>();
        }
        public LoginViewModel Login
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public CreditViewModel Credit
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CreditViewModel>();
            }
        }
        public PrintViewModel Print
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PrintViewModel>();
            }
        }
        public PinViewModel Pin
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PinViewModel>();
            }
        }
        public TaxPayerSettingVm TaxPayerSetting
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TaxPayerSettingVm>();
            }
        }
        public ParameterSettingVm ParameterSetting
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ParameterSettingVm>();
            }
        }
        public SoftwareSettingVm SoftwareSetting
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SoftwareSettingVm>();
            }
        }
        public SystemTestVm SystemTest
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SystemTestVm>();
            }
        }
        public XZPeriodicVm XZPeriodicVm
        {
            get
            {
                return ServiceLocator.Current.GetInstance<XZPeriodicVm>();
            }
        }
        public PrintSettingVm PrintSettingVm
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PrintSettingVm>();
            }
        }
        public OperationModeVm OperationModeVm
        {
            get
            {
                return ServiceLocator.Current.GetInstance<OperationModeVm>();
            }
        }
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}