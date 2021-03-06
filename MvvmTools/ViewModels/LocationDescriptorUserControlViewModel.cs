﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using MvvmTools.Models;
using MvvmTools.Services;
using MvvmTools.Utilities;
using Unity;

// ReSharper disable ConvertIfStatementToReturnStatement

namespace MvvmTools.ViewModels
{
    public class LocationDescriptorUserControlViewModel : BaseViewModel, IDataErrorInfo
    {
        #region Data

        #endregion Data

        #region Ctor and Init

        #endregion Ctor and Init

        #region Properties

        public ISolutionService SolutionService { get; set; }

        public LocationDescriptorUserControlViewModel Inherited { get; set; }

        #region Projects
        private List<ProjectModel> _projects;
        public List<ProjectModel> Projects
        {
            get { return _projects; }
            set { SetProperty(ref _projects, value); }
        }
        #endregion Projects

        #region ProjectIdentifier
        private string _projectIdentifier;
        public string ProjectIdentifier
        {
            get { return _projectIdentifier; }
            set
            {
                if (SetProperty(ref _projectIdentifier, value))
                    ResetProjectIdentifierCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion ProjectIdentifier

        #region PathOffProject
        private string _pathOffProject;
        public string PathOffProject
        {
            get { return _pathOffProject; }
            set
            {
                if (SetProperty(ref _pathOffProject, value))
                    ResetPathOffProjectCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion PathOffProject

        #region Namespace
        private string _namespace;
        public string Namespace
        {
            get { return _namespace; }
            set
            {
                if (SetProperty(ref _namespace, value))
                    ResetNamespaceCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion Namespace

        #endregion Properties

        #region Commands

        #region ResetPathOffProjectCommand
        DelegateCommand _resetPathOffProjectCommand;
        public DelegateCommand ResetPathOffProjectCommand => _resetPathOffProjectCommand ?? (_resetPathOffProjectCommand = new DelegateCommand(ExecuteResetPathOffProjectCommand, CanResetPathOffProjectCommand));
        public bool CanResetPathOffProjectCommand() => PathOffProject != Inherited?.PathOffProject;
        public void ExecuteResetPathOffProjectCommand()
        {
            PathOffProject = Inherited.PathOffProject;
        }
        #endregion

        #region ResetNamespaceCommand
        DelegateCommand _resetNamespaceCommand;
        public DelegateCommand ResetNamespaceCommand => _resetNamespaceCommand ?? (_resetNamespaceCommand = new DelegateCommand(ExecuteResetNamespaceCommand, CanResetNamespaceCommand));
        public bool CanResetNamespaceCommand() => Namespace != Inherited?.Namespace;
        public void ExecuteResetNamespaceCommand()
        {
            Namespace = Inherited.Namespace;
        }
        #endregion
        
        #region ResetProjectIdentifierCommand
        DelegateCommand _resetProjectIdentifierCommand;
        public DelegateCommand ResetProjectIdentifierCommand => _resetProjectIdentifierCommand ?? (_resetProjectIdentifierCommand = new DelegateCommand(ExecuteResetProjectIdentifierCommand, CanResetProjectIdentifierCommand));
        public bool CanResetProjectIdentifierCommand() => ProjectIdentifier != Inherited?.ProjectIdentifier;
        public void ExecuteResetProjectIdentifierCommand()
        {
            ProjectIdentifier = Inherited.ProjectIdentifier;
        }
        #endregion

        #endregion Commands

        #region Virtuals

        #endregion Virtuals

        #region Public Methods

        public void SetFromDescriptor(LocationDescriptor descriptor)
        {
            _projectIdentifier = descriptor.ProjectIdentifier;
            _pathOffProject = descriptor.PathOffProject;
            _namespace = descriptor.Namespace;
        }

        public LocationDescriptor GetDescriptor()
        {
            return new LocationDescriptor
            {
                ProjectIdentifier = ProjectIdentifier,
                PathOffProject = PathOffProject,
                Namespace = Namespace
            };
        }

        // Scans the solution and initializes.
        public async Task InitializeFromSolution()
        {
            var projects = await SolutionService.GetProjectsList();
            projects.Insert(0, new ProjectModel("(current project)", null, null, ProjectKind.Project, null, null));
            
            // Have to save and restore the project id because the XAML binding engine nulls it.
            var save = ProjectIdentifier;
            Projects = projects;
            ProjectIdentifier = save;
        }

        public bool IsInherited
        {
            get
            {
                if (Inherited == null)
                    return false;

                if (ProjectIdentifier != Inherited.ProjectIdentifier)
                    return false;
                if (PathOffProject != Inherited.PathOffProject)
                    return false;
                if (Namespace != Inherited.Namespace)
                    return false;
                if (ProjectIdentifier != Inherited.ProjectIdentifier)
                    return false;
                
                return true;
            }
        }

        public void ResetToInherited()
        {
            ProjectIdentifier = Inherited.ProjectIdentifier;
            PathOffProject = Inherited.PathOffProject;
            Namespace = Inherited.Namespace;
            ProjectIdentifier = Inherited.ProjectIdentifier;
        }

        #endregion Public Methods

        #region Private Helpers and Event Handlers

        #endregion Private Helpers and Event Handlers

        #region IDataErrorInfo

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "PathOffProject":
                        return ValidationUtilities.ValidatePathOffProject(PathOffProject);

                    case "Namespace":
                        return ValidationUtilities.ValidateNamespace(Namespace);
                }
                return null;
            }
        }
        
        public string Error => null;

        #endregion IDataErrorInfo

        public LocationDescriptorUserControlViewModel(IUnityContainer container,
            ISolutionService solutionService) 
            : base(container)
        {
            SolutionService = solutionService;
        }
    }
}