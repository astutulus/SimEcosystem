using EcosystemClassLibrary;
using System.Text;

namespace SimEco;

public partial class GUIForm : Form
{
    private bool running = true;
    private ESpecies drawToolSelected = ESpecies.grass;

    private readonly World myWorld;
    private readonly Rectangle drawingRect;

    public GUIForm()
    {
        InitializeComponent();

        GetTypicalWorld(ref myWorld, ref drawingRect);

        drawToolSelected = ESpecies.rabbit;
        PrintInstruction();

        foxTxt.Text = "";
        rabbitTxt.Text = "";
        grassTxt.Text = "";

        Thread refresh = new(RefreshScreen);
        refresh.Start();
    }

    private void GetTypicalWorld(ref World myWorld, ref Rectangle drawingRect)
    {
        // Pre-compute working area where user may click
        drawingRect = panel.ClientRectangle;
        drawingRect.Offset(panel.Location);

        drawingRect.Width -= 40;
        drawingRect.Height -= 40;

        drawingRect.X += 20;
        drawingRect.Y += 20;

        myWorld = new("Robinland", drawingRect, Constants.kSpriteDia);
    }

    /*
     * Main "game loop"
     */
    void RefreshScreen()
    {
        while (running)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(Constants.kRefreshDelayMs);
                Invalidate();
            }
            // Every tenth cycle check the following
            CheckMouseOnMap();

            // Text labels by tools
            // foxTxt.Text = LivingThing.GetSpeciesCount(ESpecies.fox).ToString();
        }
    }

    protected void CheckMouseOnMap()
    {
        Point mousePos = MousePosition;

        if (drawingRect.Contains(mousePos))
        {
            Entity nearest = World.GetNearestEntityToPointFromWorld(mousePos, myWorld);
            if (nearest != null)
            {
                PrintInfo(nearest);
            }
        }
        else
        {
            PrintInstruction();
        }
    }

    protected override void OnMouseDown(MouseEventArgs @event)
    {
        if (@event.Button == MouseButtons.Left)
        {
            if (drawingRect.Contains(@event.Location))
            {
                switch (drawToolSelected)
                {
                    case ESpecies.fox:
                        new Fox(myWorld, @event.Location);
                        break;
                    case ESpecies.rabbit:
                        new Rabbit(myWorld, @event.Location);
                        break;
                    case ESpecies.grass:
                        new Grass(myWorld, @event.Location);
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
        PaintTerrain(e);
        PaintEntities(e);
        PaintDrawToolBox(e);

        labelCount.Text = myWorld.Entities.Count.ToString();

        base.OnPaint(e);
    }

    private void PaintTerrain(PaintEventArgs e)
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

                LivingThing lt = item as LivingThing;
                switch (lt.Species)
                {
                    case ESpecies.fox:
                        {
                            using var fill = new SolidBrush(Constants.kFoxFillColour);
                            using var stroke = new Pen(Constants.kFoxStrokeColour, Constants.kFoxStrokeWidth);
                            e.Graphics.FillEllipse(fill, bounds);
                            e.Graphics.DrawEllipse(stroke, bounds);
                        }
                        break;

                    case ESpecies.rabbit:
                        {
                            using var fill = new SolidBrush(Constants.kRabbitFillColour);
                            using var stroke = new Pen(Constants.kRabbitStrokeColour, Constants.kRabbitStrokeWidth);
                            e.Graphics.FillEllipse(fill, bounds);
                            e.Graphics.DrawEllipse(stroke, bounds);
                        }
                        break;

                    case ESpecies.grass:
                        {
                            using var fill = new SolidBrush(Constants.kGrassFillColour);
                            using var stroke = new Pen(Constants.kGrassStrokeColour, Constants.kGrassStrokeWidth);
                            e.Graphics.FillEllipse(fill, bounds);
                            e.Graphics.DrawEllipse(stroke, bounds);
                        }
                        break;
                }
            }
        }
    }

    private void PaintDrawToolBox(PaintEventArgs e)
    {
        PictureBox target = null;
        switch (drawToolSelected)
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
        using var stroke = new Pen(Constants.kToolStrokeColour, Constants.kToolStrokeWidth);
        e.Graphics.DrawRectangle(stroke, rect);
    }

    private void ButtonStop_Click(object sender, EventArgs e)
    {
        running = false;
        myWorld.Entities.Clear();
        running = true;
    }

    private void pictureBox1_Click(object sender, EventArgs e)
    {
        drawToolSelected = ESpecies.fox;
        PrintInstruction();
    }

    private void pictureBox2_Click(object sender, EventArgs e)
    {
        drawToolSelected = ESpecies.rabbit;
        PrintInstruction();
    }

    private void pictureBox3_Click(object sender, EventArgs e)
    {
        drawToolSelected = ESpecies.grass;
        PrintInstruction();
    }

    private void PrintInstruction()
    {
        string placing = drawToolSelected.ToString().ToUpper();
        StringBuilder instruction = new StringBuilder();
        instruction.Append(string.Format("Click map to place {0}", placing));
        instruction.Append("\nor click a different life form");
        SafelySetLabelDescription(instruction.ToString());
    }

    private void PrintInfo(Entity ent)
    {
        string? text = ent.ToString();
        if (text != null)
        {
            SafelySetLabelDescription(text);
        }
    }

    private void ClearDescription()
    {
        SafelySetLabelDescription("");
    }

    private void SafelySetLabelDescription(string text)
    {
        if (labelDescription.InvokeRequired)
        {
            labelDescription.BeginInvoke(delegate { labelDescription.Text = text; });
        }
    }
}
