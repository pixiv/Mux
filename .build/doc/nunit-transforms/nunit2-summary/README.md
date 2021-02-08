# NUnit 2 Summary Transforms -- Charlie Poole

This folder contains a set of transforms extracted from the nunit-summary program (http://github.com/charliepoole/nunit-summary) and renamed for easier use. They essentially duplicate the output that is produced by the NUnit V2 Console runner when the test is run, extracting the necessary information from the nunit2-formatted XML result file.

The following transforms are included:

* `html-report-v2.xslt` creates a report similar to what the console itself displays in html format.
* `html-summary-v2.xslt` creates the summary report alone in html format.
* `text-report-v2.xslt` creates a report similar to what the console itself displays in text format.
* `text-summary-v2.xslt` creates the summary report alone in text format.

See our [website](http://nunit.org/nunit-summary) for samples of the report output.

All the transforms require an input XML file in NUnit V2 format. To apply the transform, you need to use a program that can apply an XSLT transform to an XML file.
