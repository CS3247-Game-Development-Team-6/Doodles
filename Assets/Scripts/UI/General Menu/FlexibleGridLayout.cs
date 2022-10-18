using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup {

    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private Vector2 spacing;

    private Vector2 cellSize;

    public override void CalculateLayoutInputHorizontal() {
        base.CalculateLayoutInputHorizontal();

        if (rows == 0 || columns == 0) {
            float sqRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqRt);
            columns = rows;
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        cellSize.x = (parentWidth - (columns - 1) * spacing.x) / (float)columns;
        cellSize.y = (parentHeight - (rows - 1) * spacing.y) / (float)rows;
        int rowCount = 0;
        int colCount = 0;

        for (int i= 0; i < rectChildren.Count; i++) {
            rowCount = i / columns;
            colCount = i % columns;
            var item = rectChildren[i];
            var xPos = (cellSize.x * colCount) + colCount * spacing.x;
            var yPos = (cellSize.y * rowCount) + rowCount * spacing.y;
            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }
    }

    public override void CalculateLayoutInputVertical() {
    }

    public override void SetLayoutHorizontal() {
    }

    public override void SetLayoutVertical() {
    }

}
