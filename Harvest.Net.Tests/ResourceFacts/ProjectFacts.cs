﻿using Harvest.Net.Models;
using System;
using System.Linq;
using Xunit;

namespace Harvest.Net.Tests
{
    public class ProjectFacts : FactBase, IDisposable
    {
        Project _todelete = null;

        [Fact]
        public void ListProjects_Returns()
        {
            var list = Api.ListProjects();

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
        }

        [Fact]
        public void Project_ReturnsProject()
        {
            var Project = Api.Project(GetTestId(TestId.ProjectId));

            Assert.NotNull(Project);
            Assert.Equal("Test", Project.Name);
        }

        [Fact]
        public void DeleteProject_ReturnsTrue()
        {
            var Project = Api.CreateProject("Test Delete Project", GetTestId(TestId.ClientId));

            var result = Api.DeleteProject(Project.Id);

            Assert.Equal(true, result);
        }

        [Fact]
        public void CreateProject_ReturnsANewProject()
        {
            _todelete = Api.CreateProject("Test Create Project", GetTestId(TestId.ClientId));

            Assert.Equal("Test Create Project", _todelete.Name);
            Assert.Equal(GetTestId(TestId.ClientId), _todelete.ClientId);
        }

        // https://github.com/harvesthq/api/issues/93 for the two tests below

        [Fact(Skip="Getting a 404. I think this end point might have been removed.")]
        public void ToggleProject_TogglesTheProjectStatus()
        {
            var _todelete = Api.CreateProject("Test Toggle Project", GetTestId(TestId.ClientId));

            Assert.Equal(true, _todelete.Active);

            var result = Api.ToggleProject(_todelete.Id);

            Assert.Equal(true, result);
            Assert.Null(Api.Project(_todelete.Id));
        }

        [Fact(Skip = "Getting a 302. I think this end point might be broken.")]
        public void UpdateProject_UpdatesOnlyChangedValues()
        {
            _todelete = Api.CreateProject("Test Update Project", GetTestId(TestId.ClientId));

            var updated = Api.UpdateProject(_todelete.Id, name: "Test Updated Project", notes: "notes");
            
            // stuff changed
            Assert.NotEqual(_todelete.Name, updated.Name);
            Assert.Equal("Test Updated Project", updated.Name);
            Assert.NotEqual(_todelete.Notes, updated.Notes);
            Assert.Equal("notes", updated.Notes);

            // stuff didn't change
            Assert.Equal(_todelete.Active, updated.Active);
            Assert.Equal(_todelete.BillBy, updated.BillBy);
            Assert.Equal(_todelete.Budget, updated.Budget);
            Assert.Equal(_todelete.ClientId, updated.ClientId);
        }

        public void Dispose()
        {
            if (_todelete != null)
                Api.DeleteProject(_todelete.Id);
        }
    }
}