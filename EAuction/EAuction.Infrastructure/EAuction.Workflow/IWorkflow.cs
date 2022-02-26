using EAuction.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAuction.Workflow
{
    public interface IWorkflow
    {
        Task<IWorkflowData> RunAsync(IWorkflowData workflowData);
        
        int Rank { get; }

        bool IsCompleted { get; }

        bool IsErrored { get; }

        string Key { get; }

        IEnumerable<IWorkflow> DepedentWorkFlows { get; }

        IWorkflow SetKey(string key);

        IWorkflow SetDependentWorkFlows(IEnumerable<IWorkflow> workflows);

        IWorkflow SetRank(int rank);
    }
}
