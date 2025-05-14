import { Card, Image, Typography } from 'antd';

const { Meta } = Card;
const { Paragraph } = Typography;

interface TopicCardProps {
  id: string;
  title: string;
  content: string;
  mediaUrl?: string;
}


const TopicCard = ({ id, title, content, mediaUrl }: TopicCardProps) => {
  return (
    <Card
      key={id}
      hoverable
      actions={[
      ]}
      styles={{ body: { textAlign: 'left' } }}
    >
      <Meta title={title} />
      {mediaUrl && (
        <div style={{ display: 'flex', justifyContent: 'center', margin: '16px 0' }}>
          <Image src={mediaUrl} alt={title} width={'80%'} preview={false} />
        </div>
      )}
      <Paragraph ellipsis={{ rows: 3 }}>{content}</Paragraph>
    </Card>
  );
};

export default TopicCard; 