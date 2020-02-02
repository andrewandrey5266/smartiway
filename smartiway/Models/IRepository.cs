using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace smartiway.Models
{
	public interface IRepository: IDisposable
	{
		IEnumerable<string> GetAll();
		void Add(string assignment);
		void Remove(int index);
		void Promote(string assignment);
		void Demote(string assignment);
		void AssignPriority(string assignment, int newPriority);
		Task Save();
		int GetPriority(string assignment);
		int GetLowestPriority { get; }
		int GetHighestPriority { get; }
	}
}