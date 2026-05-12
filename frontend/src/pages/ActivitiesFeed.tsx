import { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../app/hooks';
import { selectAllActivities, setActivities } from '../features/activities/activitiesSlice';
import { Box, Typography } from '@mui/material';
import ActivityCard from '../components/ActivityCard';
import type { ActivityEvent } from '../types/ActivityEvent';

const mockActivities: ActivityEvent[] = [
  { id: '1', title: 'Coding session', timestamp: new Date().toISOString(), sourceProvider: 'GitHub', category: 'DeepWork' },
  { id: '2', title: 'Team standup', timestamp: new Date().toISOString(), sourceProvider: 'GoogleCalendar', category: 'Social' },
];

export default function ActivitiesFeed() {
  const dispatch = useAppDispatch();
  const activities = useAppSelector(selectAllActivities);

  useEffect(() => {
    dispatch(setActivities(mockActivities));
  }, [dispatch]);

  return (
    <Box>
      <Typography variant="h4" gutterBottom>Your Activities</Typography>
      <Box
        sx={{
          display: 'grid',
          gridTemplateColumns: {
            xs: '1fr',
            sm: 'repeat(2, 1fr)',
            md: 'repeat(3, 1fr)',
          },
          gap: 2,
        }}
      >
        {activities.map((activity) => (
          <ActivityCard key={activity.id} activity={activity} />
        ))}
      </Box>
    </Box>
  );
}
