using Ninject;

namespace AMishnahADay.ViewModels {
  public class ViewModelLocator {
    public IKernel Kernel { get; set; }

    public ViewModelLocator() =>
      Kernel = new StandardKernel();
    
    public MainWindowViewModel MainWindowViewModel => Kernel.Get<MainWindowViewModel>();
    public SettingsViewModel SettingsViewModel => Kernel.Get<SettingsViewModel>();
  }
}