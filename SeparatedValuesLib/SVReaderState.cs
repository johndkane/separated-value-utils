namespace Com.PlanktonSoup.SeparatedValuesLib {

    public enum SVReaderState {
        S0_StartLine = 0,
        S1_QualificationOpener = 1,
        S2_QuoteChordStart = 2,
        S3_BuildingQualifiedValue = 3,
        S4_BuildingValue = 4,
        S5_ValueComplete = 5,
        S6_ERROR = 6,
        S7_EndLine = 7,
    }

}
