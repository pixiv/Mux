<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text"/>
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
    
    <!-- Command Line -->
    <xsl:value-of select="concat('Command Line ', $newline)"/>
    <xsl:value-of select="concat(command-line, $break)"/>

    <!-- Runtime Environment -->
    <xsl:value-of select="concat('Runtime Environment ', $newline)"/>
    <xsl:value-of select="concat('   OS Version: ', test-suite/environment/@os-version[1], $newline)"/>
    <xsl:value-of select="concat('  CLR Version: ', @clr-version, $break)"/>
    <xsl:value-of select="concat('NUnit Version: ', @engine-version, $break)"/>

    <!-- Test Files -->
    <xsl:value-of select="concat('Test Files ', $newline)"/>
    <xsl:for-each select="test-suite[@type='Assembly']">
      <xsl:value-of select="concat('    ', @fullname)"/>
      <xsl:if test="not(position() = last())">
        <xsl:value-of select="$newline"/>
      </xsl:if>
    </xsl:for-each>

    <xsl:value-of select="$break"/>
    
    <!-- Tests Not Run -->
    <xsl:if test="//test-case[@result='Skipped']">
      <xsl:value-of select="concat('Tests Not Run',$break)"/>
    </xsl:if>
    <xsl:apply-templates select="//test-case[@result='Skipped']"/>

    <!-- Errors and Failures -->
    <xsl:if test="//test-case[failure]">
      <xsl:value-of select="concat('Errors and Failures',$break)"/>
    </xsl:if>
    <xsl:apply-templates select="//test-case[failure]"/>

    <!-- Run Settings (gets first one found) -->
    <xsl:variable name="settings" select ="//settings[1]" />
    <xsl:value-of select="concat('Run Settings', $newline)"/>
    <xsl:value-of select="concat('    ','DefaultTimeout: ', $settings/setting[@name='DefaultTimeout']/@value, $newline)"/>
    <xsl:value-of select="concat('    ','WorkDirectory: ', $settings/setting[@name='WorkDirectory']/@value, $newline)"/>
    <xsl:value-of select="concat('    ','ImageRuntimeVersion: ', $settings/setting[@name='ImageRuntimeVersion']/@value, $newline)"/>
    <xsl:value-of select="concat('    ','ImageTargetFrameworkName: ', $settings/setting[@name='ImageTargetFrameworkName']/@value, $newline)"/>
    <xsl:value-of select="concat('    ','ImageRequiresX86: ', $settings/setting[@name='ImageRequiresX86']/@value, $newline)"/>
    <xsl:value-of select="concat('    ','ImageRequiresDefaultAppDomainAssemblyResolver: ', $settings/setting[@name='ImageRequiresDefaultAppDomainAssemblyResolver']/@value, $newline)"/>
    <xsl:value-of select="concat('    ','NumberOfTestWorkers: ', $settings/setting[@name='NumberOfTestWorkers']/@value)"/>
    <xsl:value-of select="$break"/>

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

  <xsl:template match="test-case">
    <!-- Determine type of test-case for formatting -->
    <xsl:variable name="type">
      <xsl:choose>
        <xsl:when test="@result='Skipped'">
          <xsl:choose>
            <xsl:when test="@label='Ignored' or @label='Explicit'">
              <xsl:value-of select="@label"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="'Other'"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:when test="@result='Failed'">
          <xsl:choose>
            <xsl:when test="@label='Error' or @label='Invalid'">
              <xsl:value-of select="@label"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="'Failed'"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="'Unknown'"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    
    <!-- Show details of test-cases either skipped or errored -->
    <xsl:value-of select="concat(position(), ') ', $type,' : ', @fullname, $newline, child::node()/message)"/>
    <xsl:choose>
      <xsl:when test="$type='Failed'">
        <xsl:value-of select="$newline"/>
      </xsl:when>
      <xsl:when test="$type='Error'">
        <xsl:value-of select="$newline"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$break"/>
      </xsl:otherwise>
    </xsl:choose>

    <!-- Stack trace for failures -->
    <xsl:if test="failure">
      <xsl:choose>
        <xsl:when test="$type='Failed'">
          <xsl:value-of select="concat(failure/stack-trace,$newline)"/>
        </xsl:when>
        <xsl:when test="$type='Error'">
          <xsl:value-of select="concat(failure/stack-trace,$break)"/>
        </xsl:when>
      </xsl:choose>
    </xsl:if>

  </xsl:template>

</xsl:stylesheet>
