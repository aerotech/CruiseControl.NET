using System;
using System.IO;
using Exortech.NetReflector;
using ThoughtWorks.CruiseControl.Core.tasks;
using ThoughtWorks.CruiseControl.Core.Util;

namespace ThoughtWorks.CruiseControl.Core.Tasks
{
	[ReflectorType("ncover")]
	public class NCoverCoverage : ICoverage
	{
		private ProcessExecutor _exec;
		private string _srcFilePath;
		private string _ncoverBinPath;
		private string _reportName= String.Empty;
		private string _ncoverReportBinPath;
		private string _nrecoverBinPath;
		private NUnitTask _nunitTask;

		public NCoverCoverage() :this(new ProcessExecutor()){}

		public NCoverCoverage(ProcessExecutor executor)
		{
			_exec = executor;
		}

		public virtual void Instrument()
		{
			string args = @"/report-name:" + _reportName + @" /recurse:" + _srcFilePath;
			Console.WriteLine("<instrument>"+_ncoverBinPath+" "+args+"</instrument>");
			RunProcess(_ncoverBinPath, args);
		}

		public virtual void Report()
		{
			try
			{
				GenerateReport();
			}
			finally
			{
				RemoveInstumentation();	
			}
		}

		public NUnitTask NUnitTask
		{
			get { return _nunitTask; }
			set { _nunitTask = value; }
		}

		private void RemoveInstumentation()
		{
			string args = "/report-name:\"" + _reportName + "\"";
			Console.WriteLine("<removeInstrumentation>"+_nrecoverBinPath+" "+args+"</removeInstrumentation>");
			RunProcess(_nrecoverBinPath, args);
		}

		private void RunProcess( string binaryPath, string args)
		{
			_exec.Execute(new ProcessInfo(binaryPath, args));
		}

		private void GenerateReport()
		{
			string actualPath = new FileInfo(_nunitTask.Assembly[0]).DirectoryName+@"\"+ "actual.xml";
			string args = "/actual:" +"\""+ actualPath+"\"" + " /report-name:" + "\"" + _reportName + "\"";
			Console.WriteLine("<report>"+_ncoverReportBinPath+" "+args+"</report>");
			RunProcess(_ncoverReportBinPath, args);
		}


		[ReflectorProperty("source")] 
		public string SrcFilePath
		{
			set { _srcFilePath = value; }
			get { return _srcFilePath; }
		}

		[ReflectorProperty("report", Required = false)] 
		public string ReportName
		{
			get { return _reportName; }
			set
			{
				if(value != String.Empty) 
					_reportName = value;
			}
		}

		[ReflectorProperty("ncoverBin")] 
		public string NCoverInstrumentBinPath
		{
			set { _ncoverBinPath = value; }
			get { return _ncoverBinPath; }
		}
		
		[ReflectorProperty("ncoverReportBin")] 
		public string NCoverReportBinPath
		{
			get { return _ncoverReportBinPath; }
			set { _ncoverReportBinPath = value; }
		}
		
		[ReflectorProperty("nrecoverBin")] 
		public string NRecoverPath
		{
			get { return _nrecoverBinPath; }
			set { _nrecoverBinPath = value; }
		}
	}
}