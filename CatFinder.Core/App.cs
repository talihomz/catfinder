using Catfinder.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;

namespace Catfinder.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            // auto register services we will need for this
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            // register the first screen to show
            RegisterAppStart<FinderViewModel>();
        }
    }
}
