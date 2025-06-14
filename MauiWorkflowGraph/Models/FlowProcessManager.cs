using System;
using System.Collections.Generic;

namespace MauiWorkflowGraph.Models
{
    public sealed class FlowProcessManager
    {
        private static readonly Lazy<FlowProcessManager> _instance = new(() => new FlowProcessManager());
        private readonly Dictionary<string, FlowProcess> _flowProcesses;
        private FlowProcess _selectedProcess;

        // Private constructor to prevent instantiation
        private FlowProcessManager()
        {
            _flowProcesses = new Dictionary<string, FlowProcess>
            {
                { "Rule1", new FlowProcess( "Rule 1", "Description for Rule 1", "(a, b) => a * b", "Result1") },
                { "Rule2", new FlowProcess( "Rule 2", "Description for Rule 2", "(a, b) => a * b", "Result2") },
                { "Rule3", new FlowProcess( "Rule 3", "Description for Rule 3", "(a, b) => a * b", "Result3") },
                { "Rule4", new FlowProcess( "Rule 4", "Description for Rule 4", "(a, b) => a * b", "Result4") },
                { "Rule5", new FlowProcess( "Rule 5", "Description for Rule 5", "(a, b) => a * b", "Result5") },
                { "Rule6", new FlowProcess( "Rule 6", "Description for Rule 6", "(a, b) => a * b", "Result6") },
                { "Rule7", new FlowProcess( "Rule 7", "Description for Rule 7", "(a, b) => a * b", "Result7") },
                { "Rule8", new FlowProcess( "Rule 8", "Description for Rule 8", "(a, b) => a * b", "Result8") },
                { "Rule9", new FlowProcess( "Rule 9", "Description for Rule 9", "(a, b) => a * b", "Result9") },
                { "Rule10", new FlowProcess( "Rule 10", "Description for Rule 10", "(a, b) => a * b", "Result10") },
                { "Rule11", new FlowProcess( "Rule 11", "Description for Rule 11", "(a, b) => a * b", "Result11") },
                { "Rule12", new FlowProcess( "Rule 12", "Description for Rule 12", "(a, b) => a * b", "Result12") },
                { "Rule13", new FlowProcess( "Rule 13", "Description for Rule 13", "(a, b) => a * b", "Result13") },
                { "Rule14", new FlowProcess( "Rule 14", "Description for Rule 14", "(a, b) => a * b", "Result14") },
                { "Rule15", new FlowProcess( "Rule 15", "Description for Rule 15", "(a, b) => a * b", "Result15") }
            };
        }

        // Public property to access the singleton instance
        public static FlowProcessManager Instance => _instance.Value;

        // Method to get the dictionary of rules
        public Dictionary<string, FlowProcess> FlowProcesses => _flowProcesses;

        public FlowProcess SelectedProcess
        {
            get
            {
                return _selectedProcess;
            }
            set
            {
                if (_selectedProcess != value)
                {
                    _selectedProcess = value;
                    // Notify property changed if using a framework that supports it
                    // OnPropertyChanged(nameof(SelectedProcess));
                }
            }
        }
    }
}
