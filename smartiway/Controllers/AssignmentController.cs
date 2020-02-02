using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using smartiway.Models;
using smartiway.Repositories.Interfaces;

namespace smartiway.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AssignmentController : ControllerBase
	{
		private readonly IRepository _assignmentRepository;

		public AssignmentController(IRepository assignmentRepository)
		{
			_assignmentRepository = assignmentRepository;
		}

		[HttpGet]
		public ActionResult<IEnumerable<Assignment>> Get()
		{
			return Ok(_assignmentRepository.GetAll().Select((x, i) => new Assignment {Priority = i, Text = x}));
		}
		
		[HttpPost]
		public async Task<ActionResult<Assignment>> Post(Assignment assignment)
		{
			if (assignment == null)
			{
				return new BadRequestResult();
			}

			if (_assignmentRepository.GetAll().Contains(assignment.Text))
			{
				return BadRequest("Assignment already exists in list");
			}
 
			_assignmentRepository.Add(assignment.Text);
			await _assignmentRepository.Save();
			
			assignment.Priority = _assignmentRepository.GetPriority(assignment.Text);
			return Ok(assignment);
		}
 
		[HttpPut]
		[Route("promote")]
		public async Task<ActionResult<Assignment>> Promote(Assignment assignment)
		{
			if (!_assignmentRepository.GetAll().Contains(assignment.Text))
			{
				return BadRequest("Assignment isn't in list");
			}
			
			if (_assignmentRepository.GetPriority(assignment.Text) == _assignmentRepository.GetHighestPriority)
			{
				return BadRequest("Assigment has already highest priority");
			}
			_assignmentRepository.Promote(assignment.Text);
			await _assignmentRepository.Save();
			
			assignment.Priority = _assignmentRepository.GetPriority(assignment.Text);
			return Ok(assignment);
		}
		
		[HttpPut]
		[Route("demote")]
		public async Task<ActionResult<Assignment>> Demote(Assignment assignment)
		{
			if (!_assignmentRepository.GetAll().Contains(assignment.Text))
			{
				return BadRequest("Assignment isn't in list");
			}
			
			if (_assignmentRepository.GetPriority(assignment.Text) == _assignmentRepository.GetLowestPriority)
			{
				return BadRequest("Assigment has already lowest priority");
			}
			_assignmentRepository.Demote(assignment.Text);
			await _assignmentRepository.Save();
			
			assignment.Priority = _assignmentRepository.GetPriority(assignment.Text);
			return Ok(assignment);
		}
		
		[HttpPut]
		[Route("prioritize")]
		public async Task<ActionResult<Assignment>> Prioritize(Assignment assignment)
		{
			if (!_assignmentRepository.GetAll().Contains(assignment.Text))
			{
				return BadRequest("Assignment isn't in list");
			}

			if (_assignmentRepository.GetPriority(assignment.Text) == assignment.Priority)
			{
				return BadRequest("Assigment has already this priority");
			}

			if (assignment.Priority < _assignmentRepository.GetHighestPriority
			    || assignment.Priority > _assignmentRepository.GetLowestPriority)
			{
				return BadRequest("Assignment's new priority is out of boundaries");
			}
			
			_assignmentRepository.AssignPriority(assignment.Text, assignment.Priority);
			await _assignmentRepository.Save();
			
			assignment.Priority = _assignmentRepository.GetPriority(assignment.Text);
			return Ok(assignment);
		}
 
		[HttpDelete("{priority}")]
		public async Task<ActionResult<Assignment>> Delete(int priority)
		{
			if (priority < _assignmentRepository.GetHighestPriority
			    || priority > _assignmentRepository.GetLowestPriority)
			{
				return BadRequest("Assignment's priority is out of boundaries");
			}

			string assignment = _assignmentRepository.GetAll().ToList().ElementAtOrDefault(priority);
			_assignmentRepository.Remove(priority);
			await _assignmentRepository.Save();

			return Ok(new Assignment {Text = assignment});
		}
	}
}