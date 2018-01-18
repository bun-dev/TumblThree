﻿using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows.Threading;

using TumblThree.Applications.Properties;
using TumblThree.Applications.Services;
using TumblThree.Applications.ViewModels;
using TumblThree.Domain;
using TumblThree.Domain.Queue;

namespace TumblThree.Applications.Controllers
{
    [Export(typeof(IModuleController))][Export]
    internal class ModuleController : IModuleController
    {
        private const string appSettingsFileName = "Settings.json";
        private const string managerSettingsFileName = "Manager.json";
        private const string queueSettingsFileName = "Queuelist.json";


        private readonly Lazy<CrawlerController> crawlerController;
        private readonly Lazy<DetailsController> detailsController;
        private readonly IEnvironmentService environmentService;
        private readonly Lazy<ManagerController> managerController;
        private readonly Lazy<QueueController> queueController;
        private readonly QueueManager queueManager;
        private readonly ISettingsProvider settingsProvider;

        private readonly Lazy<ShellService> shellService;
        private readonly Lazy<ShellViewModel> shellViewModel;
        private AppSettings appSettings;
        private ManagerSettings managerSettings;
        private QueueSettings queueSettings;


        [ImportingConstructor]
        public ModuleController(Lazy<ShellService> shellService, IEnvironmentService environmentService,
            ISettingsProvider settingsProvider, Lazy<ManagerController> managerController,
            Lazy<QueueController> queueController, Lazy<DetailsController> detailsController,
            Lazy<CrawlerController> crawlerController, Lazy<ShellViewModel> shellViewModel)
        {
            this.shellService = shellService;
            this.environmentService = environmentService;
            this.settingsProvider = settingsProvider;
            this.detailsController = detailsController;
            this.managerController = managerController;
            this.queueController = queueController;
            this.crawlerController = crawlerController;
            this.shellViewModel = shellViewModel;
            queueManager = new QueueManager();
        }

        private ShellService ShellService => shellService.Value;

	    private ManagerController ManagerController => managerController.Value;

	    private QueueController QueueController => queueController.Value;

	    private DetailsController DetailsController => detailsController.Value;

	    private CrawlerController CrawlerController => crawlerController.Value;

	    private ShellViewModel ShellViewModel => shellViewModel.Value;

	    public void Initialize()
        {
            if (CheckIfPortableMode(appSettingsFileName))
            {
                appSettings = LoadSettings<AppSettings>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, appSettingsFileName));
                queueSettings = LoadSettings<QueueSettings>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, queueSettingsFileName));
                managerSettings = LoadSettings<ManagerSettings>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, managerSettingsFileName));
				
            }
            else
            {
                appSettings = LoadSettings<AppSettings>(Path.Combine(environmentService.AppSettingsPath, appSettingsFileName));
                queueSettings = LoadSettings<QueueSettings>(Path.Combine(environmentService.AppSettingsPath, queueSettingsFileName));
                managerSettings = LoadSettings<ManagerSettings>(Path.Combine(environmentService.AppSettingsPath, managerSettingsFileName));
	            
            }

            ShellService.Settings = appSettings;
            ShellService.ShowErrorAction = ShellViewModel.ShowError;
            ShellService.ShowDetailsViewAction = ShowDetailsView;
            ShellService.ShowQueueViewAction = ShowQueueView;
            ShellService.UpdateDetailsViewAction = UpdateDetailsView;
            ShellService.InitializeOAuthManager();

            ManagerController.QueueManager = queueManager;
            ManagerController.ManagerSettings = managerSettings;
            ManagerController.BlogManagerFinishedLoadingLibrary += OnBlogManagerFinishedLoadingLibrary;
            Task managerControllerInit = ManagerController.Initialize();
            QueueController.QueueSettings = queueSettings;
            QueueController.QueueManager = queueManager;
            QueueController.Initialize();
            DetailsController.QueueManager = queueManager;
            DetailsController.Initialize();
            CrawlerController.QueueManager = queueManager;
            CrawlerController.Initialize();
        }

        public async void Run()
        {
            ShellViewModel.IsQueueViewVisible = true;
            ShellViewModel.Show();

            // Let the UI to initialize first before loading the queuelist.
            await Dispatcher.CurrentDispatcher.InvokeAsync(QueueController.Run, DispatcherPriority.ApplicationIdle);
        }

        public void Shutdown()
        {
            DetailsController.Shutdown();
            QueueController.Shutdown();
            ManagerController.Shutdown();
            CrawlerController.Shutdown();

            if (appSettings.PortableMode)
            {
                SaveSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, appSettingsFileName), appSettings);
                SaveSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, queueSettingsFileName), queueSettings);
                SaveSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, managerSettingsFileName), managerSettings);
	            
            }
            else
            {
                SaveSettings(Path.Combine(environmentService.AppSettingsPath, appSettingsFileName), appSettings);
                SaveSettings(Path.Combine(environmentService.AppSettingsPath, queueSettingsFileName), queueSettings);
                SaveSettings(Path.Combine(environmentService.AppSettingsPath, managerSettingsFileName), managerSettings);
	            
            }
        }

        private void OnBlogManagerFinishedLoadingLibrary(object sender, EventArgs e)
        {
            QueueController.LoadQueue();
        }

        private static bool CheckIfPortableMode(string fileName)
        {
            return File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));
        }

        private T LoadSettings<T>(string fileName) where T : class, new()
        {
            try
            {
                return settingsProvider.LoadSettings<T>(fileName);
            }
            catch (Exception ex)
            {
                Logger.Error("Could not read the settings file: {0}", ex);
                return new T();
            }
        }

        private void SaveSettings(string fileName, object settings)
        {
            try
            {
                settingsProvider.SaveSettings(fileName, settings);
            }
            catch (Exception ex)
            {
                Logger.Error("Could not save the settings file: {0}", ex);
            }
        }

        private void ShowDetailsView()
        {
            ShellViewModel.IsDetailsViewVisible = true;
        }

        private void ShowQueueView()
        {
            ShellViewModel.IsQueueViewVisible = true;
        }

        private void UpdateDetailsView()
        {
            if (!ShellViewModel.IsQueueViewVisible)
            {
	            ShellViewModel.IsDetailsViewVisible = true;
            }
        }
    }
}
