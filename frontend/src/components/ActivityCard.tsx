import {
  Card,
  CardContent,
  Typography,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
} from "@mui/material";
import type { SelectChangeEvent } from "@mui/material";
import type { ActivityEvent } from "../types/ActivityEvent";
import { updateActivityCategory } from "../features/activities/activitiesSlice";
import { useAppDispatch } from "../app/hooks";

interface Props {
  activity: ActivityEvent;
}

export default function ActivityCard({ activity }: Props) {
  const dispatch = useAppDispatch();

  const handleCategoryChange = (e: SelectChangeEvent) => {
    dispatch(
      updateActivityCategory({
        id: activity.id,
        changes: { category: e.target.value },
      }),
    );
    // In real app, call API to persist
  };

  return (
    <Card>
      <CardContent>
        <Typography variant="h6">{activity.title}</Typography>
        <Typography color="text.secondary">
          {activity.sourceProvider}
        </Typography>
        <Typography variant="body2">
          {new Date(activity.timestamp).toLocaleString()}
        </Typography>
        <FormControl fullWidth margin="normal">
          <InputLabel>Category</InputLabel>
          <Select
            value={activity.category}
            label="Category"
            onChange={handleCategoryChange}
          >
            <MenuItem value="DeepWork">Deep Work</MenuItem>
            <MenuItem value="Social">Social</MenuItem>
            <MenuItem value="Health">Health</MenuItem>
            <MenuItem value="Admin">Admin</MenuItem>
            <MenuItem value="Learning">Learning</MenuItem>
            <MenuItem value="Entertainment">Entertainment</MenuItem>
          </Select>
        </FormControl>
      </CardContent>
    </Card>
  );
}
