﻿using System.Collections.Generic;
using MvvmTools.Services;
using MvvmTools.Utilities;

namespace MvvmTools.Models
{
    /// <summary>
    /// Represents a solution, solution folder, project, or a project folder.
    /// </summary>
    public class ProjectModel
    {
        public string Name { get; }
        public string FullPath { get; }
        public List<ProjectModel> Children { get; }
        public string ProjectIdentifier { get; }
        public ProjectKind Kind { get; }
        public string KindId { get; }
        public string TypeDescription { get; }
        public string RootNamespace { get; }

        public ProjectModel(string name, string fullPath, string projectIdentifier, ProjectKind kind, string projectKindId, string rootNamespace)
        {
            Name = name;
            FullPath = fullPath;
            Kind = kind;
            KindId = projectKindId;
            ProjectIdentifier = projectIdentifier;
            RootNamespace = rootNamespace;

            Children = new List<ProjectModel>();

            TypeDescription = VsUtilities.GetProjectTypeDescription(projectKindId);
        }

        public override string ToString()
        {
            return $"{Name} ({FullPath})";
        }

        public string SettingsFile
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(FullPath))
                {
                    var fn = FullPath + SettingsService.SettingsFileExtension;
                    return fn;
                }
                return null;
            }
        }
    }

    public enum ProjectKind
    {
        // The actual solution.
        Solution,
        // A project. 
        Project,
        // A solution folder (which may contain other folders).
        SolutionFolder,
        // A folder within a project (which cannot contain anything but files).
        ProjectFolder,
        // A simple project item.
        Item
    }
}
