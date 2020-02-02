using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace smartiway.Models
{
	public class AssignmentRepository : IRepository
	{
		private List<string> Assignments { get; }
		
		public AssignmentRepository()
		{
			Assignments = File.ReadAllLines(Directory.GetCurrentDirectory() + "/Resources.txt").ToList();

		}

		public IEnumerable<string> GetAll()
		{
			return Assignments;
		}

		public void Add(string assignment)
		{
			Assignments.Add(assignment);
		}

		public void Remove(int index)
		{
			Assignments.RemoveAt(index);
		}

		public void Promote(string assignment)
		{
			int priority = Assignments.IndexOf(assignment);
			Assignments.Remove(assignment);
			Assignments.Insert(priority - 1, assignment);
		}
		
		public void Demote(string assignment)
		{
			int priority = Assignments.IndexOf(assignment);
			Assignments.Remove(assignment);
			Assignments.Insert(priority + 1, assignment);
		}
		
		public void AssignPriority(string assignment, int newPriority)
		{
			Assignments.Remove(assignment);
			Assignments.Insert(newPriority, assignment);
		}
		
		public Task Save()
		{
			using (var sw = new StreamWriter(Directory.GetCurrentDirectory() + "/Resources.txt"))
			{
				foreach (var assignment in Assignments)
				{
					sw.WriteLine(assignment);
				}
			}

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			
		}

		public int GetPriority(string assignment)
		{
			return Assignments.IndexOf(assignment);
		}

		public int GetLowestPriority => Assignments.Count - 1;
		
		public int GetHighestPriority => 0;
	}
}