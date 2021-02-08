<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method='text'/>
  <xsl:template match="/">
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:variable name="newline">
    <xsl:text>&#xD;&#xA;</xsl:text>
  </xsl:variable>

  <xsl:variable name="break">
    <xsl:text>&#xD;&#xA;&#xD;&#xA;</xsl:text>
  </xsl:variable>

  <xsl:template match="test-run">

    <!-- Runtime Environment -->
    <xsl:value-of select="concat('Runtime Environment ', $newline)"/>
    <xsl:value-of select="concat('   OS Version: ', test-suite/environment/@os-version[1], $newline)"/>
    <xsl:value-of select="concat('  CLR Version: ', @clr-version, $break)"/>
    <xsl:value-of select="concat('NUnit Version: ', @engine-version, $break)"/>

    <!-- Test Run Summary -->
    <xsl:value-of select="concat('Test Run Summary ', $newline)"/>
    <xsl:value-of select="concat(' Overall result: ', @result, $newline)"/>
    <xsl:value-of select="concat(' Test Count: ', @total, ', Passed: ', @passed, ', Failed: ', @failed, ', Inconclusive: ', @inconclusive, ', Skipped: ', @skipped, $newline)"/>

    <!-- [Optional] - Failed Test Summary -->
    <xsl:if test="@failed > 0">
      <xsl:variable name="failedTotal" select="count(//test-case[@result='Failed' and not(@label)])" />
      <xsl:variable name="errorsTotal" select="count(//test-case[@result='Failed' and @label='Error'])" />
      <xsl:variable name="invalidTotal" select="count(//test-case[@result='Failed' and @label='Invalid'])" />
      <xsl:value-of select="concat('   Failed Tests - Failures: ', $failedTotal, ', Errors: ', $errorsTotal, ', Invalid: ', $invalidTotal, $newline)"/>
    </xsl:if>

    <!-- [Optional] - Skipped Test Summary -->
    <xsl:if test="@skipped > 0">
      <xsl:variable name="ignoredTotal" select="count(//test-case[@result='Skipped' and @label='Ignored'])" />
      <xsl:variable name="explicitTotal" select="count(//test-case[@result='Skipped' and @label='Explicit'])" />
      <xsl:variable name="otherTotal" select="count(//test-case[@result='Skipped' and not(@label='Explicit' or @label='Ignored')])" />
      <xsl:value-of select="concat('   Skipped Tests - Ignored: ', $ignoredTotal, ', Explicit: ', $explicitTotal, ', Other: ', $otherTotal, $newline)"/>
    </xsl:if>

    <!-- Times -->
    <xsl:value-of select="concat(' Start time: ', @start-time, $newline)"/>
    <xsl:value-of select="concat('   End time: ', @end-time, $newline)"/>
    <xsl:value-of select="concat('   Duration: ', format-number(@duration,'0.000'), ' seconds', $break)"/>

  </xsl:template>

</xsl:stylesheet>
