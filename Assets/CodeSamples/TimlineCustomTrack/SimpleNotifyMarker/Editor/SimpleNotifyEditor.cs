using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace Emptybraces.Timeline
{
    [CustomTimelineEditor(typeof(SimpleNotifyMarker))]
    public class SimpleNotifyEditor : MarkerEditor
    {
        // デフォのマーカーは消せないから、カスタムするなら、隠すような画像にする必要がある。
        // const string k_OverlayPath = "eventmarker";
        // const string k_OverlaySelectedPath = "eventmarker_selected";
        // const string k_OverlayCollapsedPath = "timeline_annotation_overlay_collapsed";
        // static Texture2D s_OverlayTexture;
        // static Texture2D s_OverlaySelectedTexture;
        // static Texture2D s_OverlayCollapsedTexture;
        // static List<EventMarker> s_dupList;

        static SimpleNotifyEditor()
        {
            // s_dupList = new();
            // s_OverlayTexture = Resources.Load<Texture2D>(k_OverlayPath);
            // s_OverlaySelectedTexture = Resources.Load<Texture2D>(k_OverlaySelectedPath);
            // s_OverlayCollapsedTexture = Resources.Load<Texture2D>(k_OverlayCollapsedPath);
        }
        // Draws a vertical line on top of the Timeline window's contents.
        public override void DrawOverlay(IMarker marker, MarkerUIStates uiState, MarkerOverlayRegion region)
        {
            if (marker is not SimpleNotifyMarker e)
                return;
            // s_dupList.RemoveAll(elem => elem == null || e == elem);
            // s_dupList.Add(e);
            DrawLineOverlay(e, region);
            // DrawColorOverlay(region, e.ShowLineOverlayColor, uiState);
        }

        // Sets the marker's tooltip based on its title.
        public override MarkerDrawOptions GetMarkerOptions(IMarker marker)
        {
            if (marker is not SimpleNotifyMarker e)
                return base.GetMarkerOptions(marker);
            return new MarkerDrawOptions { tooltip = e.Description };
        }

        static void DrawLineOverlay(SimpleNotifyMarker e, MarkerOverlayRegion region)
        {
            // int total = 0;
            // for (int i = 0, l = s_dupList.Count; i < l; ++i)
            // {
            //     if (e.Frame == s_dupList[i].Frame)
            //     {
            //         ++total;
            //     }
            // }
            // if (2 <= total)
            // {
            // var w = 6 / total;
            // float x = region.markerRegion.xMin + (region.markerRegion.width - w) / 2.0f;
            // var half = total / 2;
            // var isOdd = total % 2 != 0;
            // for (int i = -half, j = 0; i <= half; i++)
            // {
            //     if (!isOdd && i == 0)// 偶数の場合は中心をスキップ
            //         continue;
            //     var ee = s_dupList[j++];
            //     DrawLineOverlay(x + i * w * 10, w, ee.ShowLineOverlayColor, region, total);
            // }
            // return;
            // }

            // Calculate markerRegion's center on the x axis
            if (e.ShowLineOverlay)
            {
                var width = e.ShowLineOverlayWidth;
                float markerRegionCenterX = region.markerRegion.xMin + (region.markerRegion.width - width) / 2.0f;
                DrawLineOverlay(markerRegionCenterX, width, e.ShowLineOverlayColor, region);
            }
            // if (1 < total)
            // {
            //     EditorGUI.IntField(new Rect(region.markerRegion.xMin + 4, region.timelineRegion.y + 22, 20, 20), total, EditorStyles.boldLabel);
            // }
        }
        static void DrawLineOverlay(float x, float width, Color color, MarkerOverlayRegion region)
        {
            Rect overlayLineRect = new Rect(x, region.timelineRegion.y, width, region.timelineRegion.height);
            Color overlayLineColor = new Color(color.r, color.g, color.b, color.a * 0.5f);
            EditorGUI.DrawRect(overlayLineRect, overlayLineColor);
        }

        // static void DrawColorOverlay(MarkerOverlayRegion region, Color color, MarkerUIStates state)
        // {
        //     // Save the Editor's overlay color before changing it
        //     Color oldColor = GUI.color;
        //     GUI.color = color;

        //     if (state.HasFlag(MarkerUIStates.Selected))
        //     {
        //         GUI.DrawTexture(region.markerRegion, s_OverlaySelectedTexture);
        //     }
        //     else if (state.HasFlag(MarkerUIStates.Collapsed))
        //     {
        //         // GUI.DrawTexture(region.markerRegion, s_OverlayCollapsedTexture);
        //     }
        //     else 
        //     {
        //         GUI.DrawTexture(region.markerRegion, s_OverlayTexture);
        //     }

        //     // Restore the previous Editor's overlay color
        //     GUI.color = oldColor;
        // }
    }
}
