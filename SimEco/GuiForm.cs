using EcosystemClassLibrary;
using System.Drawing;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SimEco;

public partial class GUIForm : Form
{
    private readonly World myWorld;
    private readonly Rectangle worldEdge;

    private bool _running = true;
    private Entity? _nearestToCursor = null;
    private ESpecies _toolSelected = ESpecies.grass;

    // So I fixed a hard bug, by moving this out of the only function that uses it.
    // To do: understand why it can't be in that fnction.
    private Point mousePos;

    public GUIForm()
    {
        InitializeComponent();

        worldEdge = panel.ClientRectangle;
        worldEdge.Width -= Constants.kMargin;
        worldEdge.Height -= Constants.kMargin;
        worldEdge.Offset(Constants.kMargin / 2, Constants.kMargin / 2);

        myWorld = new("Robinland", worldEdge);

        _toolSelected = ESpecies.rabbit;
        foxTxt.Text = "";
        rabbitTxt.Text = "";
        grassTxt.Text = "";

        PrintInstruction();

        Thread refresh = new(Run);
        refresh.Start();
    }

    void Run()
    {
        while (_running)
        {
            Thread.Sleep(Constants.kRefreshDelayMs);
            ActOnMousePos();
            UpdateTextValues();
            Invalidate();
        }
    }

    private void ActOnMousePos()
    {

        if (panel.InvokeRequired)
        {
            panel.BeginInvoke(delegate { mousePos = panel.PointToClient(Cursor.Position); });
        }
        else
        {
            mousePos = panel.PointToClient(Cursor.Position);
        }

        if (worldEdge.Contains(mousePos))
        {
            if (myWorld.Entities.Count > 0)
            {
                _nearestToCursor = World.GetNearestEntityToPointFromWorld(mousePos, myWorld);
                PrintInfo(_nearestToCursor);
            }
        }
        else
        {
            _nearestToCursor = null;
            PrintInstruction();
        }
    }

    protected override void OnMouseDown(MouseEventArgs @event)
    {
        if (@event.Button == MouseButtons.Left)
        {
            var clickPos = panel.PointToClient(Cursor.Position);

            if (myWorld.Extents.Contains(clickPos))
            {
                switch (_toolSelected)
                {
                    case ESpecies.fox:
                        new Fox(clickPos);
                        break;
                    case ESpecies.rabbit:
                        new Rabbit(clickPos);
                        break;
                    case ESpecies.grass:
                        new Grass(clickPos);
                        break;
                    default:
                        ClearDescription();
                        break;
                }
            }
        }
        base.OnMouseDown(@event);
    }

    private void UpdateTextValues()
    {
        // Text labels by tools
        // foxTxt.Text = LivingThing.GetSpeciesCount(ESpecies.fox).ToString();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        PaintMap(e);
        PaintEntities(e);
        PaintDrawToolBox(e);
        PaintEntityHighlight(e);

        labelCount.Text = myWorld.Entities.Count.ToString();

        base.OnPaint(e);
    }

    private void PaintMap(PaintEventArgs e)
    {
        Rectangle rect = panel.ClientRectangle;
        rect.Offset(panel.Location);

        using var fill = new SolidBrush(Constants.kTerrainFillColour);
        using var stroke = new Pen(Constants.kTerrainStrokeColour, Constants.kTerrainStrokeWidth);

        e.Graphics.FillRectangle(fill, rect);
        e.Graphics.DrawRectangle(stroke, rect);

    }

    private void PaintEntities(PaintEventArgs e)
    {
        foreach (Entity ent in myWorld.Entities)
        {
            if (ent is LivingThing)
            {
                LivingThing? item = ent as LivingThing;

                if (item is not null)
                {
                    int dia = (int)item.Mass;

                    Point pt = new Point(item.Position.X - (dia / 2), item.Position.Y - (dia / 2));
                    Size sz = new Size(dia, dia);
                    Rectangle bounds = new(pt, sz);
                    bounds.Offset(panel.Location);

                    SolidBrush fill;
                    Pen stroke;

                    LivingThing lt = item as LivingThing;
                    switch (lt.Species)
                    {
                        case ESpecies.fox:
                            {
                                fill = new SolidBrush(Constants.kFoxFillColour);
                                stroke = new Pen(Constants.kFoxStrokeColour, Constants.kFoxStrokeWidth);
                            }
                            break;

                        case ESpecies.rabbit:
                            {
                                fill = new SolidBrush(Constants.kRabbitFillColour);
                                stroke = new Pen(Constants.kRabbitStrokeColour, Constants.kRabbitStrokeWidth);
                            }
                            break;

                        case ESpecies.grass:
                            {
                                fill = new SolidBrush(Constants.kGrassFillColour);
                                stroke = new Pen(Constants.kGrassStrokeColour, Constants.kGrassStrokeWidth);
                            }
                            break;
                        default:
                            {
                                fill = new SolidBrush(Color.White); // defaults (not used)
                                stroke = new Pen(Color.White, 0);   // defaults (not used)
                            }
                            break;
                    }

                    using var usingFill = fill;
                    using var usingStroke = stroke;
                    e.Graphics.FillEllipse(usingFill, bounds);
                    e.Graphics.DrawEllipse(usingStroke, bounds);
                }
            }
        }
    }

    private void PaintDrawToolBox(PaintEventArgs e)
    {
        PictureBox target = null;
        switch (_toolSelected)
        {
            case ESpecies.fox:
                target = pictureBox1;
                break;
            case ESpecies.rabbit:
                target = pictureBox2;
                break;
            case ESpecies.grass:
                target = pictureBox3;
                break;
        }
        Rectangle rect = target.ClientRectangle;
        rect.Offset(target.Location);
        using var stroke = new Pen(Constants.kToolHighlightColour, Constants.kToolStrokeWidth);
        e.Graphics.DrawRectangle(stroke, rect);
    }

    private void PaintEntityHighlight(PaintEventArgs e)
    {
        if (_nearestToCursor is LivingThing)
        {
            LivingThing? highlighted = _nearestToCursor as LivingThing;

            if (highlighted is not null)
            {
                int mass = (int)highlighted.Mass;

                int dimension = mass + Constants.kHighlightMargin * 2;
                Size highlightSize = new(dimension, dimension);

                Point highlightLocation = highlighted.Position;
                highlightLocation.Offset(panel.Location);
                highlightLocation.X -= mass / 2 + Constants.kHighlightMargin;
                highlightLocation.Y -= mass / 2 + Constants.kHighlightMargin;

                Rectangle highlight = new(highlightLocation, highlightSize);

                using var stroke = new Pen(Constants.kInfoHighlightColour, Constants.kHighlightStrokeWidth);
                e.Graphics.DrawRectangle(stroke, highlight);
            }
        }
    }

    private void ButtonStop_Click(object sender, EventArgs e)
    {
        _running = false;
        myWorld.Entities.Clear();
        _running = true;
    }

    private void pictureBox1_Click(object sender, EventArgs e)
    {
        _toolSelected = ESpecies.fox;
        PrintInstruction();
    }

    private void pictureBox2_Click(object sender, EventArgs e)
    {
        _toolSelected = ESpecies.rabbit;
        PrintInstruction();
    }

    private void pictureBox3_Click(object sender, EventArgs e)
    {
        _toolSelected = ESpecies.grass;
        PrintInstruction();
    }

    private void PrintInstruction()
    {
        string placing = _toolSelected.ToString().ToUpper();
        StringBuilder instruction = new StringBuilder();
        instruction.Append(string.Format("Click map to place {0}", placing));
        instruction.Append("\nor click a different life form");
        SafelySetLabelDescription(instruction.ToString(), Constants.kToolHighlightColour);
    }

    private void PrintInfo(Entity ent)
    {
        string? text = ent.ToString();
        if (text != null)
        {
            SafelySetLabelDescription(text, Constants.kInfoHighlightColour);
        }
    }

    private void ClearDescription()
    {
        SafelySetLabelDescription("", Constants.kNoColour);
    }

    private void SafelySetLabelDescription(string text, Color colour)
    {
        if (labelDescription.InvokeRequired)
        {
            labelDescription.ForeColor = colour;
            labelDescription.BeginInvoke(delegate { labelDescription.Text = text; });
        }
        else
        {
            labelDescription.ForeColor = colour;
            labelDescription.Text = ". " + text;
        }
    }
}
