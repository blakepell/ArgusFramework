    Public Function ExtractKeywords(text As String) As String
        Dim analzyer As New SemanticLibrary.KeywordAnalyzer
        Dim anaylsis As SemanticLibrary.KeywordAnalysis = analzyer.Analyze(text)
        Dim sb As New StringBuilder

        ' Top 10
        For Each key As SemanticLibrary.Keyword In (From n In anaylsis.Keywords).Take(10)
            sb.AppendFormat("{0} ---> {1}{2}", key.Word, key.Rank, vbCrLf)
        Next

        ' All
        For Each key As SemanticLibrary.Keyword In anaylsis.Keywords
            sb.AppendFormat("{0} {1}{2}", key.Word, key.Rank, vbCrLf)
        Next

        Return sb.ToString

    End Function

	        public string ExtractKeywords(string text)
        {
            int rank = 0;
            var analzyer = new KeywordAnalyzer();
            var anaylsis = analzyer.Analyze(text);
            StringBuilder sb = new StringBuilder();

            // Top 10
            foreach (var key in anaylsis.Keywords.Take(10))
            {
                rank++;
                sb.AppendFormat("{0}.) {1}: {2}\r\n", rank, key.Word, key.Rank);
            }

            // All
            foreach (var key in anaylsis.Keywords)
            {
                sb.AppendFormat("{0} {1}\r\n", key.Word, key.Rank);
            }

            return sb.ToString();

        }