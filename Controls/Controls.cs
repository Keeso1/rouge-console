using System.Drawing;
using Vimonia.Enums;
using Vimonia.World;

public static class Controls{

    public static Point Move(Direction? direction, Point PrevPosition, TileMap CurrentRoom){
        Point position = direction switch
        {
            Direction.Down => PrevPosition with
            {
                Y = Math.Clamp(PrevPosition.Y + 1, 0, CanvasWrapper.Instance.Size.Height - 1),
            },
            Direction.Up => PrevPosition with {
                Y = Math.Clamp(PrevPosition.Y - 1, 0, CanvasWrapper.Instance.Size.Height - 1),
            },
            Direction.Left => PrevPosition with {
                X = Math.Clamp(PrevPosition.X - 1, 0, CanvasWrapper.Instance.Size.Width - 1),
            },
            Direction.Right => PrevPosition with {
                X = Math.Clamp(PrevPosition.X + 1, 0, CanvasWrapper.Instance.Size.Width - 1),
            },
            _ => PrevPosition,
        };

        if(CurrentRoom.Tiles[position.X, position.Y].Walkable){
            return position;
        }else{
            return PrevPosition;
        }
    }


    public static Point Move(Direction? direction, Point PrevPosition, TileMap CurrentRoom, Point playerPos){
        Point position = direction switch
        {
            Direction.Down => PrevPosition with
            {
                Y = Math.Clamp(PrevPosition.Y + 1, 1, CanvasWrapper.Instance.Size.Height - 2),
            },
            Direction.Up => PrevPosition with {
                Y = Math.Clamp(PrevPosition.Y - 1, 1, CanvasWrapper.Instance.Size.Height - 2),
            },
            Direction.Left => PrevPosition with {
                X = Math.Clamp(PrevPosition.X - 1, 1, CanvasWrapper.Instance.Size.Width - 2),
            },
            Direction.Right => PrevPosition with {
                X = Math.Clamp(PrevPosition.X + 1, 1, CanvasWrapper.Instance.Size.Width - 2),
            },
            _ => PrevPosition,
        };
        if(CurrentRoom.Tiles[position.X, position.Y].Walkable){
            return position;
        }else{
            return PrevPosition;
        }
    }
}
