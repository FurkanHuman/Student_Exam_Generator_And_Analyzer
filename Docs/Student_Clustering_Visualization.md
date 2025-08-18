# Student Clustering & Visualization

This document explains how student-level data can be transformed into meaningful features and visualized through clustering and complementary chart types. The goal is to support S.E.S with interpretable dashboards for teachers and administrators.

---

## Feature → Visualization Mapping

| **Feature** | **Preprocessing** | **Metric** | **Recommended Visualization** | **Notes** |
|-------------|-------------------|------------|-------------------------------|-----------|
| Student performance per exam / topic | Normalize to percentages, z-score | Euclidean / Ward linkage | Dendrogram, Heatmap | Reveals natural groups of students with similar success patterns. |
| Error-type distribution (correct, wrong, blank, partial) | Convert to proportions (sums to 1) | Cosine | Stacked Bar, Dendrogram | Shows *how* students fail; useful for remediation. |
| Topic or skill-based averages | Aggregate by learning area | Euclidean | Radar Chart, Heatmap | Clear view of strengths/weaknesses per student. |
| Temporal progression (exam history) | Trend calculation, moving average | Euclidean / Correlation | Line Chart | Identifies improvement or decline over time. |
| Demographic comparison (class, school, gender) | One-hot encode or annotate only | Not used for clustering directly | Grouped Bar, Box Plot, Annotated Dendrogram | Use as labels to interpret clusters, not as cluster features. |
| Misconception patterns (frequent wrong options) | Build sparse vectors of wrong-choice frequency | Jaccard / Cosine | Network Graph, Heatmap | Highlights shared misconceptions across students. |

---

**Summary:**  
- **Dendrogram + Heatmap** → overall class grouping  
- **Radar Chart** → individual student profile  
- **Stacked Bar / Box Plot** → error types & demographic comparisons  
- **Line Chart** → performance trends  
