<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
		<html>
			<head>
				<meta charset="utf-8"></meta>
				<title>My Plants</title>
			</head>
			<body>
				<h1>Specimens I Have Found</h1>
				<table border="1">
					<tr>
						<th>Latitude</th>
						<th>Longitude</th>
						<th>Planted By</th>
						<th>Comments</th>
					</tr>
					<xsl:for-each select="/plant/specimens/specimen">
						<tr>
							<td>
								<xsl:value-of select="latitude"/>
							</td>
							<td>
								<xsl:value-of select="longitude"/>
							</td>
							<td>
								<xsl:value-of select="planted_by"/>
							</td>
							<td>
								<xsl:value-of select="comments"/>
							</td>
						
						</tr>
					</xsl:for-each>
				</table>
			</body>
		
		</html>
	</xsl:template>
</xsl:stylesheet>