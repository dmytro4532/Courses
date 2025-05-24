import { Card, Image, Typography, Button, Tag, Space } from 'antd';
import type { Topic } from '../../../types';

const { Meta } = Card;
const { Paragraph } = Typography;

interface TopicCardProps {
  topic: Topic;
  isCompleted?: boolean;
  canComplete?: boolean;
  onComplete?: () => void;
}

const TopicCard = ({ topic, isCompleted, canComplete, onComplete }: TopicCardProps) => {
  return (
    <Card
      key={topic.id}
      hoverable
      styles={{ body: { textAlign: 'left' } }}
      actions={[

      ]}
    >
      <Meta title={topic.title} />
      {topic.mediaUrl && (
        <div style={{ display: 'flex', justifyContent: 'center', margin: '16px 0' }}>
          <Image src={topic.mediaUrl} alt={topic.title} width={'80%'} preview={false} />
        </div>
      )}
      <Paragraph ellipsis={{ rows: 3 }}>{topic.content}</Paragraph>

      <Space>
        {
          isCompleted ? (
            <Tag color="green" key="completed">Completed</Tag>
          ) : canComplete ? (
            <Button
              key="complete"
              type="primary"
              size="small"
              onClick={onComplete}
            >
              Mark as Completed
            </Button>
          ) : null
        }
      </Space>
    </Card>
  );
};

export default TopicCard;