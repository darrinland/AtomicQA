using AtomicWriter;
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
							TestCases = new List<Test>()
							{
								new Test()
								{
									TestName = "TC01",
								},
								new Test()
								{
									TestName = "TC02",
								},
								new Test()
								{
									TestName = "TC03",
								},
								new Test()
								{
									TestName = "TC04",
								}
							}
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
							TestCases = new List<Test>()
							{
								new Test()
								{
									TestName = "TC01",
								},
								new Test()
								{
									TestName = "TC02",
								},
								new Test()
								{
									TestName = "TC03",
								},
							}
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
			foreach (var module in _modules)
			{
				CreateModuleExpander(module);
			}

			var firstModule = _modules.FirstOrDefault();
			if (firstModule != null)
			{
				((Expander) ModuleExpanders.Children[0]).IsExpanded = true;

			}
		}

		private void CreateModuleExpander(Module module)
		{
			var moduleExpander = new Expander()
			{
				Content = new StackPanel() { },
				Header = module.Name,
				DataContext = module
			};

			foreach (var requirement in module.Requirements)
			{
				var reqButton = new Button()
				{
					Content = requirement.Title,
					DataContext = requirement

				};
				reqButton.Click += RequirementButtonHandler;

				((StackPanel)moduleExpander.Content).Children.Add(reqButton);
			}

			ModuleExpanders.Children.Add(moduleExpander);
		}

		private void RequirementButtonHandler(object sender, RoutedEventArgs e)
		{
			Requirement req = (Requirement) ((Button) sender).DataContext;
			//Clear Side Panel
			TestCases.Children.Clear();

			//Show Selected Details
			RequirementTitle.Text = req.Title;
			RequirementDetails.Text = req.Details;
			
			foreach (var testCase in req.TestCases)
			{
				var tcButton = new Button()
				{
					Content = testCase.TestName,
					DataContext = testCase
				};
				TestCases.Children.Add(tcButton);
			}
		}
		
		private void AddModuleHandler(object sender, RoutedEventArgs e)
		{
			Module newMod = new Module()
			{
				Name = "New Module",
				Requirements = new List<Requirement>(),
			};

			_modules.Add(newMod);

			CreateModuleExpander(newMod);
		}

        private void NavigateToWelcome_Click(object sender, RoutedEventArgs e)
        {
            var projectSelection = new WelcomeScreen();
            projectSelection.Show();
            this.Close();
        }
    }
}
