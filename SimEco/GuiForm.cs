using EcosystemClassLibrary;

namespace SimEco;

public partial class GUIForm : Form
{
    private bool running = true;
    private ESpecies drawToolSelected = ESpecies.grass;

    private readonly World world;
    private readonly Rectangle drawingRect;

    public GUIForm()
    {
        InitializeComponent();

        #region Pre-computation
        // Pre-compute working area where user may click
        drawingRect = panel.ClientRectangle;
        drawingRect.Offset(panel.Location);

        drawingRect.Width -= 40;
        drawingRect.Height -= 40;

        drawingRect.X += 20;
        drawingRect.Y += 20;


        world = new("Robinland", drawingRect, Constants.kSpriteDia);


        #endregion

        drawToolSelected = ESpecies.rabbit;
        PrintInstruction("herbivore");

        foxTxt.Text = "";
        rabbitTxt.Text = "";
        grassTxt.Text = "";

        Thread refresh = new(RefreshScreen);
        refresh.Start();
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

            // Text labels by tools
            // foxTxt.Text = LivingThing.GetSpeciesCount(ESpecies.fox).ToString();
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
                        new Fox(world, @event.Location);
                        break;
                    case ESpecies.rabbit:
                        new Rabbit(world, @event.Location);
                        break;
                    case ESpecies.grass:
                        new Grass(world, @event.Location);
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

        labelCount.Text = world.Entities.Count.ToString();

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
        foreach (Entity item in world.Entities)
        {
            if (item is LivingThing)
            {
                int dia = Constants.kSpriteDia;

                Point pt = new Point(item.Position.X - (dia / 2), item.Position.Y - (dia / 2));
                Size sz = new Size(dia, dia);
                Rectangle bounds = new (pt, sz);

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
        world.Entities.Clear();
        running = true;
    }

    private void pictureBox1_Click(object sender, EventArgs e)
    {
        drawToolSelected = ESpecies.fox;
        PrintInstruction("carnivore");
    }

    private void pictureBox2_Click(object sender, EventArgs e)
    {
        drawToolSelected = ESpecies.rabbit;
        PrintInstruction("herbivore");
    }

    private void pictureBox3_Click(object sender, EventArgs e)
    {
        drawToolSelected = ESpecies.grass;
        PrintInstruction("vegetation");
    }

    private void PrintInstruction(string text)
    {
        labelDescription.Text = string.Format("Click map to place {0},\nor click a different life form", text.ToUpper());
    }
    private void ClearDescription()
    {
        labelDescription.Text = "";
    }
}
