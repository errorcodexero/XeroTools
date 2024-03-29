#include "PathGenerator.h"
#include "TrajectorySamplePoint.h"
#include "pathfinder.h"

namespace xero
{
	namespace paths
	{

		PathGenerator::PathGenerator(int steps, double timestep)
		{
			steps_ = steps;
			timestep_ = timestep;
		}

		PathGenerator::~PathGenerator()
		{
		}

		TimedTrajectory PathGenerator::generate(const std::vector<Pose2d>& pts, double maxvel, double maxaccel, double maxjerk)
		{


			Waypoint* points = (Waypoint*)malloc(sizeof(Waypoint) * pts.size());

			for (size_t i = 0; i < pts.size(); i++)
			{
				points[i].x = pts[i].getTranslation().getX();
				points[i].y = pts[i].getTranslation().getY();
				points[i].angle = pts[i].getRotation().toRadians();
			}

			TrajectoryCandidate candidate;
			pathfinder_prepare(points, static_cast<int>(pts.size()), FIT_HERMITE_QUINTIC, steps_, timestep_, maxvel, maxaccel, maxjerk, &candidate);
			free(points);

			int length = candidate.length;
			Segment* trajectory = (Segment *)malloc(length * sizeof(Segment));

			pathfinder_generate(&candidate, trajectory);

			std::vector<TimedTrajectoryPoint> ptarr;
			double time = 0.0;

			for (size_t i = 0; i < static_cast<size_t>(length); i++)
			{
				Pose2d p2d(trajectory[i].x, trajectory[i].y, Rotation2d::fromRadians(trajectory[i].heading));
				Pose2dWithCurvature p2dwc(p2d, 0.0, 0.0);
				TrajectorySamplePoint sp(p2dwc, 0, 0);
				TimedTrajectoryPoint pt(sp, time, trajectory[i].position, trajectory[i].velocity, trajectory[i].acceleration, trajectory[i].jerk);
				ptarr.push_back(pt);
				time += trajectory[i].dt;
			}

			free(trajectory);
			TimedTrajectory ret(ptarr);
			return ret;
		}
	}
}
