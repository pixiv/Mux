# NUnit 3 Summary Transforms -- Charlie Poole

This folder contains a set of transforms extracted from the nunit-summary program (http://github.com/charliepoole/nunit-summary) and renamed for easier use. They essentially duplicate the output that is produced by the NUnit 3 Console runner when the test is run, extracting the necessary information from the nunit3-formatted XML result file.

The following transforms are included:

* `html-report.xslt` creates a report similar to what the console itself displays in html format.
* `html-summary.xslt` creates the summary report alone in html format.
* `text-report.xslt` creates a report similar to what the console itself displays in text format.
* `text-summary.xslt` creates the summary report alone in text format.'

See our [website](http://nunit.org/nunit-summary) for samples of the report output.

These transforms may be used independently or through the `nunit3-console` `--result` option. When used with `nunit3-console`, use a command-line similar to this:

```
nunit3-console.exe my.test.dll --result=my.test.summary.txt;transform=text-summary.xslt
```

Naturally, if you use one of the HTML transforms, you will want to change the file type of the result output.

Note that the `--result` option may be repeated to create several reports. If you use the above command-line, the default `TestResult.xml` will not be saved. If you want it you should use a command like this:

```
nunit3-console.exe my.test.dll --result=my.test.summary.txt;transform=text-summary.xslt --result=TestResult.xml
```

If you want to use one of the transforms separately, after the test run, you will need to use a program that can apply an XSLT transform to an XML file. Note that the input file must be in NUnit3 format.
