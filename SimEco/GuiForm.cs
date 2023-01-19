using EcosystemClassLibrary;
using System.Text;

namespace SimEco;

public partial class GUIForm : Form
{
    private bool _running = true;
    private Boolean _isNearestHighlighted = false;
    private Entity? _nearestEntity = null;
    private ESpecies _toolSelected = ESpecies.grass;

    private readonly Rectangle worldEdge;
    private readonly World myWorld;

    public GUIForm()
    {
        InitializeComponent();

        worldEdge = panel.ClientRectangle;
        worldEdge.Width -= Constants.kSpriteDia;
        worldEdge.Height -= Constants.kSpriteDia;
        worldEdge.Offset(Constants.kSpriteDia / 2, Constants.kSpriteDia / 2);

        myWorld = new("Robinland", worldEdge, Constants.kSpriteDia);

        _toolSelected = ESpecies.rabbit;
        foxTxt.Text = "";
        rabbitTxt.Text = "";
        grassTxt.Text = "";

        PrintInstruction();

        Thread refresh = new(RefreshScreen);
        refresh.Start();
    }

    /*
     * Main "game loop"
     */
    void RefreshScreen()
    {
        while (_running)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(Constants.kRefreshDelayMs);
                Invalidate();
            }
            // Every tenth cycle check the following

            // Text labels by tools
            // foxTxt.Text = LivingThing.GetSpeciesCount(ESpecies.fox).ToString();
        }
    }

    protected override void OnMouseMove(MouseEventArgs @event)
    {
        var mousePos = panel.PointToClient(Cursor.Position);

        if (worldEdge.Contains(mousePos))
        {
            if (myWorld.Entities.Count > 0)
            {
                _isNearestHighlighted = true;
                _nearestEntity = World.GetNearestEntityToPointFromWorld(mousePos, myWorld);
                PrintInfo(_nearestEntity);
            }
        }
        else
        {
            PrintInstruction();
            _isNearestHighlighted = false;
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
                        new Fox(myWorld, clickPos);
                        break;
                    case ESpecies.rabbit:
                        new Rabbit(myWorld, clickPos);
                        break;
                    case ESpecies.grass:
                        new Grass(myWorld, clickPos);
                        break;
                    default:
                        ClearDescription();
                        break;
                }
            }
        }
        base.OnMouseDown(@event);
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
        foreach (Entity item in myWorld.Entities)
        {
            if (item is LivingThing)
            {
                int dia = Constants.kSpriteDia;

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
        if (_isNearestHighlighted)
        {
            Point location = _nearestEntity.Position;
            location.Offset(panel.Location);
            location.X -= Constants.kHighlightSize / 2;
            location.Y -= Constants.kHighlightSize / 2;

            Size size = new(Constants.kHighlightSize, Constants.kHighlightSize);

            Rectangle highlight = new(location, size);

            using var stroke = new Pen(Constants.kInfoHighlightColour, Constants.kHighlightStrokeWidth);
            e.Graphics.DrawRectangle(stroke, highlight);
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
