using AtomicWriter.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AtomicQA
{
	/// <summary>
	/// Interaction logic for Modules.xaml
	/// </summary>
	public partial class Modules : Window
	{
		private List<Module> _modules;
		
		public Modules()
		{
			InitializeComponent();

			_modules = new List<Module>()
			{
				new Module()
				{
					Name = "Planning",
					Requirements = new List<Requirement>()
					{
						new Requirement()
						{
							Title = "The user shall create a new requirement",
							Details = "Blah blah QA Jargon about nonsense that developers have to do on their behalf which the qa peeps appreciate or whatevs",
							TestCases = new List<Test>(){}
						}
					}
				},
				new Module()
				{
					Name = "Solicitation",
					Requirements = new List<Requirement>()
					{
						new Requirement()
						{
							Title = "Create Solicitation",
							Details = "Do the other thing",
							TestCases = new List<Test>(){}
						},
						new Requirement()
						{
							Title = "Publish Solicitation",
							Details = "Do the other new thing",
							TestCases = new List<Test>(){}
						}
					}
				}
			};

			LoadModules();
		}

		private void LoadModules()
		{
			StackPanel modulesPanel = ModuleList;
			foreach (var module in _modules)
			{
				var moduleButton = new Button()
				{
					Content = module.Name,
					DataContext = module
				};
				moduleButton.Click += ModuleButtonHandler;
				modulesPanel.Children.Add(moduleButton);
			}

			var firstModule = _modules.FirstOrDefault();
			if (firstModule != null)
			{
				ShowModulePage(firstModule);
			}
		}
		
		private void ModuleButtonHandler(object sender, RoutedEventArgs e)
		{
			Button modButton = (Button)sender;
			TextBlock moduleName = ModuleName;
			moduleName.Text = modButton.Content.ToString();
			ShowModulePage((Module)modButton.DataContext);
		}

		private void ShowModulePage(Module selectedModule)
		{
			ModuleName.Text = selectedModule.Name + " Requirements";

			//Clear Requirements
			StackPanel reqPanel = RequirementList;
			reqPanel.Children.Clear();
			RequirementName.Text = "";
			RequirementDescription.Text = "";

			AddRequirement.DataContext = selectedModule;

			//Display Requirements
			StackPanel requirementPanel = RequirementList;
			foreach (var requirement in selectedModule.Requirements)
			{
				var requirementButton = new Button()
				{
					Content = requirement.Title,
					DataContext = requirement
				};
				requirementButton.Click += RequirementButtonHandler;
				requirementPanel.Children.Add(requirementButton);
			}

			var firstReq = selectedModule.Requirements.FirstOrDefault();
			if (firstReq != null)
			{
				TextBlock reqName = RequirementName;
				reqName.Text = firstReq.Title;
				TextBlock reqDescription = RequirementDescription;
				reqDescription.Text = firstReq.Details;
			}
		}

		private void RequirementButtonHandler(object sender, RoutedEventArgs e)
		{
			Button reqButton = (Button)sender;
			TextBlock reqName = RequirementName;
			reqName.Text = reqButton.Content.ToString();
			TextBlock reqDescription = RequirementDescription;
			reqDescription.Text = ((Requirement)reqButton.DataContext).Details;
		}

		private void AddModuleHandler(object sender, RoutedEventArgs e)
		{
			StackPanel modulesPanel = ModuleList;
			Module newMod = new Module()
			{
				Name = "New Module",
				Requirements = new List<Requirement>(),
			};

			_modules.Add(newMod);

			var moduleButton = new Button()
			{
				Content = newMod.Name,
				DataContext = newMod
			}; 

			moduleButton.Click += ModuleButtonHandler;
			modulesPanel.Children.Add(moduleButton);
		}

		private void AddRequirement_OnClick(object sender, RoutedEventArgs e)
		{
			StackPanel requirementPanel = RequirementList;
			Button addReqButton = (Button) sender;
			Module module = (Module) addReqButton.DataContext;

			Requirement newReq = new Requirement()
			{
				Title = "New Requirement",
				Details = "New Description",
				TestCases = new List<Test>()
			};
			module.Requirements.Add(newReq);

			var requirementButton = new Button()
			{
				Content = newReq.Title,
				DataContext = newReq
			};
			requirementButton.Click += RequirementButtonHandler;

			requirementPanel.Children.Add(requirementButton);
		}
	}
}
