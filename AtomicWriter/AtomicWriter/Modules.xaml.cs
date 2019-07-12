using AtomicWriter;
using AtomicWriter.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AtomicWriter.DataAccess;
using MahApps.Metro.Controls;

namespace AtomicQA
{
	/// <summary>
	/// Interaction logic for Modules.xaml
	/// </summary>
	public partial class Modules : Window
	{
		private Project _project;
		private Module _selectedModule;
		private Requirement _selectedRequirement;

		private void SaveProject()
		{
			DataWriter.Save(@"C:/Tests/TestProject.json", _project);
		}

		private void LoadProject()
		{
			var location = @"C:/Tests/TestProject.json";
			_project = DataReader.LoadObject<Project>(location);
		}

		public Modules()
		{
			InitializeComponent();
			LoadProject();

			//var modules = new List<Module>()
			//{
			//	new Module()
			//	{
			//		Name = "Planning",
			//		Requirements = new List<Requirement>()
			//		{
			//			new Requirement()
			//			{
			//				Title = "The user shall create a new requirement",
			//				Details = "Blah blah QA Jargon about nonsense that developers have to do on their behalf which the qa peeps appreciate or whatevs",
			//				TestCases = new List<Test>()
			//				{
			//					new Test()
			//					{
			//						TestName = "TC01",
			//					},
			//					new Test()
			//					{
			//						TestName = "TC02",
			//					},
			//					new Test()
			//					{
			//						TestName = "TC03",
			//					},
			//					new Test()
			//					{
			//						TestName = "TC04",
			//					}
			//				}
			//			}
			//		}
			//	},
			//	new Module()
			//	{
			//		Name = "Solicitation",
			//		Requirements = new List<Requirement>()
			//		{
			//			new Requirement()
			//			{
			//				Title = "Create Solicitation",
			//				Details = "Do the other thing",
			//				TestCases = new List<Test>()
			//				{
			//					new Test()
			//					{
			//						TestName = "TC01",
			//					},
			//					new Test()
			//					{
			//						TestName = "TC02",
			//					},
			//					new Test()
			//					{
			//						TestName = "TC03",
			//					},
			//				}
			//			},
			//			new Requirement()
			//			{
			//				Title = "Publish Solicitation",
			//				Details = "Do the other new thing",
			//				TestCases = new List<Test>(){}
			//			}
			//		}
			//	}
			//};

			//_project = new Project()
			//{
			//	Name = "Helix1",
			//	Modules = modules
			//};

			LoadModules();

			var firstModule = _project.Modules.FirstOrDefault();
			if (firstModule != null)
			{
				foreach (Expander m in ModuleExpanders.Children)
				{
					m.IsExpanded = true;
				}
				if (firstModule.Requirements.Count > 0)
				{
					DisplayRequirement(firstModule.Requirements.FirstOrDefault());
				}
			}
		}

		private void LoadModules()
		{
			ModuleExpanders.Children.Clear();
			foreach (var module in _project.Modules)
			{
				CreateModuleExpander(module);
			}
		}

		private void CreateModuleExpander(Module module)
		{
			var expanderHeader = new StackPanel(){Orientation = Orientation.Horizontal};
			expanderHeader.Children.Add(new TextBlock()
			{
				Text = module.Name,
				Foreground = new SolidColorBrush(Colors.White),
				FontSize = 14
			});
			var editModuleButton = new Button()
			{
				Content = "✏",
				Padding = new Thickness(0, 0, 0, 0),
				DataContext = module,
				Background = new SolidColorBrush(Colors.Transparent),
				Foreground = new SolidColorBrush(Colors.White),
				Width = 20,
				Height = 20,
				FontSize = 10,
				Margin = new Thickness(20, 0, 0, 0)
			};
			editModuleButton.Click += EditModule_click;
			expanderHeader.Children.Add(editModuleButton);

			var deleteModuleButton = new Button()
			{
				Content = "🚫",
				Padding = new Thickness(0, 0, 0, 0),
				DataContext = module,
				Background = new SolidColorBrush(Colors.Transparent),
				Foreground = new SolidColorBrush(Colors.White),
				Width = 20,
				Height = 20,
				FontSize = 10,
				Margin = new Thickness(5, 0, 0, 0)
			};
			deleteModuleButton.Click += DeleteModule_click;
			expanderHeader.Children.Add(deleteModuleButton);

			var requirementsStack = new StackPanel();

			var moduleExpander = new Expander()
			{
				Content = new StackPanel() { },
				Header = expanderHeader,
				DataContext = module,
				//Name = module.Name
			};

			((StackPanel) moduleExpander.Content).Children.Add(requirementsStack);

			var addRequirementButton = new Button()
			{
				Content = "+ Add Requirement",
				DataContext = module,
				Margin = new Thickness(0, 2, 0, 2),
				Background = new SolidColorBrush(Colors.GreenYellow)
			};
			addRequirementButton.Click += AddRequirementButtonHandler;

			((StackPanel) moduleExpander.Content).Children.Add(addRequirementButton);

			foreach (var requirement in module.Requirements)
			{
				var reqButton = new Button()
				{
					Content = requirement.Title,
					DataContext = requirement,
					Margin = new Thickness(0, 2, 0, 2)
				};
				reqButton.Click += RequirementButtonHandler;

				requirementsStack.Children.Add(reqButton);
			}

			ModuleExpanders.Children.Add(moduleExpander);
		}

		private void AddRequirementButtonHandler(object sender, RoutedEventArgs e)
		{
			Button addButton = (Button) sender;
			var module = (Module) addButton.DataContext;
			var requirement = new Requirement()
			{
				Title = "New Requirement",
				Details = "New Requirement Details",
				TestCases = new List<Test>()
			};
			module.Requirements.Add(requirement);
			LoadModules();
			foreach (Expander m in ModuleExpanders.Children)
			{
				m.IsExpanded = true;
			}
			DisplayRequirement(requirement);
			SaveProject();
		}

		private void EditModule_click(object sender, RoutedEventArgs e)
		{
			var module = (Module) ((Button) sender).DataContext;
			var namePrompt = new NamePrompt();
			namePrompt.NameInputBox.Text = module.Name;
			namePrompt.ShowDialog();
			var didSave = namePrompt.DialogResult;
			if (didSave == true)
			{
				module.Name = namePrompt.NewName;
				LoadModules();
				foreach (Expander m in ModuleExpanders.Children)
				{
					m.IsExpanded = true;
				}
				SaveProject();
			}
		}

		private void DeleteModule_click(object sender, RoutedEventArgs e)
		{
			var module = (Module)((Button)sender).DataContext;

			string messageBoxText = "Are you sure you want to delete " + module.Name + "?";
			string caption = "Remove " + module.Name;
			MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
			MessageBoxImage icnMessageBox = MessageBoxImage.Warning;
			MessageBoxResult result = MessageBox.Show(messageBoxText, caption, btnMessageBox, icnMessageBox);

			if (result == MessageBoxResult.Yes)
			{
				_project.Modules.Remove(module);
				SaveProject();
				LoadModules();
				foreach (Expander m in ModuleExpanders.Children)
				{
					m.IsExpanded = true;
				}
			}
		}

		private void RequirementButtonHandler(object sender, RoutedEventArgs e)
		{
			Requirement req = (Requirement)((Button)sender).DataContext;
			DisplayRequirement(req);
		}

		private void DisplayRequirement(Requirement requirement)
		{
			_selectedModule = _project.Modules.FirstOrDefault(x => x.Requirements.Contains(requirement));
			_selectedRequirement = requirement;

			//Clear Side Panel
			TestCases.Children.Clear();

			//Show Selected Details
			RequirementTitle.Text = requirement.Title;
			RequirementDetails.Text = requirement.Details;
			
			foreach (var testCase in requirement.TestCases)
			{
				var tcButton = new Button()
				{
					Content = testCase.TestName,
					DataContext = testCase,
					Margin = new Thickness(0, 2, 0, 2)
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

			_project.Modules.Add(newMod);
			SaveProject();

			CreateModuleExpander(newMod);
		}

		private void NavigateToWelcome_Click(object sender, RoutedEventArgs e)
		{
			var projectSelection = new WelcomeScreen();
			projectSelection.Show();
			this.Close();
		}

		private void EditRequirementTitle_OnClick(object sender, RoutedEventArgs e)
		{
			var namePrompt = new NamePrompt();
			namePrompt.NameInputBox.Text = _selectedRequirement.Title;
			namePrompt.ShowDialog();
			var didSave = namePrompt.DialogResult;
			if (didSave == true)
			{
				_selectedRequirement.Title = namePrompt.NewName;
				LoadModules(); foreach (Expander m in ModuleExpanders.Children)
				{
					m.IsExpanded = true;
				}
				DisplayRequirement(_selectedRequirement);
				SaveProject();
			}
		}

		private void EditRequirementDescription_OnClick(object sender, RoutedEventArgs e)
		{
			var namePrompt = new NamePrompt();
			namePrompt.NameInputBox.Text = _selectedRequirement.Details;
			namePrompt.ShowDialog();
			var didSave = namePrompt.DialogResult;
			if (didSave == true)
			{
				_selectedRequirement.Details = namePrompt.NewName;
				DisplayRequirement(_selectedRequirement);
				SaveProject();
			}
		}
	}
}
