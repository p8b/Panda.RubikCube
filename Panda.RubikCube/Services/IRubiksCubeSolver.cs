using System;
using System.Threading.Tasks;

using Panda.RubikCube.Entities;

namespace Panda.RubikCube.Services
{
    public interface IRubiksCubeSolver
    {
        Task<bool> Solve(RubiksCube rubiksCube, Func<Task>? moveExecuted);
    }
}