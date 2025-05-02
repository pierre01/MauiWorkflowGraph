using System;
using System.Collections.Generic;

namespace MauiWorkflowGraph.Models
{
    public sealed class RulesManager
    {
        private static readonly Lazy<RulesManager> _instance = new(() => new RulesManager());
        private readonly Dictionary<string, Rule> _rules;
        private Rule _selectedRule;

        // Private constructor to prevent instantiation
        private RulesManager()
        {
            _rules = new Dictionary<string, Rule>
            {
                { "Rule1", new Rule("Rule1", "Rule 1", "Description for Rule 1", "Expression1", "Result1") },
                { "Rule2", new Rule("Rule2", "Rule 2", "Description for Rule 2", "Expression2", "Result2") },
                { "Rule3", new Rule("Rule3", "Rule 3", "Description for Rule 3", "Expression3", "Result3") },
                { "Rule4", new Rule("Rule4", "Rule 4", "Description for Rule 4", "Expression4", "Result4") },
                { "Rule5", new Rule("Rule5", "Rule 5", "Description for Rule 5", "Expression5", "Result5") },
                { "Rule6", new Rule("Rule6", "Rule 6", "Description for Rule 6", "Expression6", "Result6") },
                { "Rule7", new Rule("Rule7", "Rule 7", "Description for Rule 7", "Expression7", "Result7") },
                { "Rule8", new Rule("Rule8", "Rule 8", "Description for Rule 8", "Expression8", "Result8") },
                { "Rule9", new Rule("Rule9", "Rule 9", "Description for Rule 9", "Expression9", "Result9") },
                { "Rule10", new Rule("Rule10", "Rule 10", "Description for Rule 10", "Expression10", "Result10") },
                { "Rule11", new Rule("Rule11", "Rule 11", "Description for Rule 11", "Expression11", "Result11") },
                { "Rule12", new Rule("Rule12", "Rule 12", "Description for Rule 12", "Expression12", "Result12") },
                { "Rule13", new Rule("Rule13", "Rule 13", "Description for Rule 13", "Expression13", "Result13") },
                { "Rule14", new Rule("Rule14", "Rule 14", "Description for Rule 14", "Expression14", "Result14") },
                { "Rule15", new Rule("Rule15", "Rule 15", "Description for Rule 15", "Expression15", "Result15") }
            };
        }

        // Public property to access the singleton instance
        public static RulesManager Instance => _instance.Value;

        // Method to get the dictionary of rules
        public Dictionary<string, Rule> Rules => _rules;
        public Rule SelectedRule
        {
            get
            {
                return _selectedRule;
            }
            set
            {
                if (_selectedRule != value)
                {
                    _selectedRule = value;
                    // Notify property changed if using a framework that supports it
                    // OnPropertyChanged(nameof(SelectedRule));
                }
            }
        }
    }
}
