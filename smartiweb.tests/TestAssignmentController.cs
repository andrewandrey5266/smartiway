using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using smartiway.Controllers;

namespace smartiweb.tests
{
	public class TestAssignmentController
	{
		[Test]
        public void Get_ShouldReturnAllAssignments()
        {
	        AssignmentController controller = new AssignmentController(new MockRepository());
            List<string> testAssignments = GetTestAssignments();

            var result = (controller.Get().Result as OkObjectResult)?.Value as IEnumerable<Assignment>;
            Assert.AreEqual(testAssignments.Count, result.Count());
        }
        
        [Test]
        public void Add_ShouldAddAssignment()
        {
	        AssignmentController controller = new AssignmentController(new MockRepository());

	        Assignment post = (controller.Post(new Assignment() {Text = "take a nap"})
		        .Result.Result as OkObjectResult)?.Value as Assignment;
	        
	        Assert.AreEqual(4, post.Priority);
        }
        
        [Test]
        public void Delete_ShouldDeleteLastAssignment()
        {
	        AssignmentController controller = new AssignmentController(new MockRepository());
            var testAssignments = GetTestAssignments();

            var result = (controller.Delete(3)
	            .Result.Result as OkObjectResult)?.Value as Assignment;
            
            Assert.AreEqual(  testAssignments.Last(), result.Text);
        }

        [Test]
        public void Promote_ShouldPromote()
        {
	        AssignmentController controller = new AssignmentController(new MockRepository());
	        var testAssignments = GetTestAssignments();

	        var testAssignment = new Assignment {Text = testAssignments.Skip(1).First()};
	        Assignment assignment = (controller.Promote(testAssignment)
		        .Result.Result as OkObjectResult)?.Value as Assignment;
	        
	        Assert.AreEqual(0, assignment.Priority);
        }

        [Test]
        public void Demote_ShouldDemote()
        {
	        AssignmentController controller = new AssignmentController(new MockRepository());
	        var testAssignments = GetTestAssignments();

	        var testAssignment = new Assignment {Text = testAssignments.Skip(1).First()};
	        Assignment assignment = (controller.Demote(testAssignment)
		        .Result.Result as OkObjectResult)?.Value as Assignment;

		        
	        Assert.AreEqual(2, assignment.Priority);
        }

        [Test]
        public void Prioritize_ShouldPrioritize()
        {
	        AssignmentController controller = new AssignmentController(new MockRepository());
	        var testAssignments = GetTestAssignments();

	        var testAssignment = new Assignment {Text = testAssignments.Skip(1).First(), Priority = 3};
	        Assignment assignment = (controller.Prioritize(testAssignment)
		        .Result.Result as OkObjectResult)?.Value as Assignment;

	        Assert.AreEqual(3, assignment.Priority);
        }
       
        private List<string> GetTestAssignments()
        {
	        var testAssignments = new List<string>
	        {
		        "Running",
		        "Eating",
		        "TakingOutTheTrash",
		        "LearnAngular"
	        };
	        
            return testAssignments;
        }
    }
}