namespace SmallSteps.Tests.ApplicationTest.Trackers
{
    using SmallSteps.Application.Trackers;
    using SmallSteps.Domain.Trackers;
    using SmallSteps.Tests.ApplicationTest.TestDoubles;
    using Xunit;

    public class GetAllTrackersUseCaseTest
    {
        [Fact]
        public void Execute_ShouldReturnAllTrackers()
        {
            // Arrange
            InMemoryTrackerRepository repository = new();

            Tracker firstTracker = new("Exercise");
            Tracker secondTracker = new("Vocalisation");

            repository.Add(firstTracker);
            repository.Add(secondTracker);

            GetAllTrackersUseCase useCase = new(repository);

            // Act
            var result = useCase.Execute();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(firstTracker, result);
            Assert.Contains(secondTracker, result);
        }

        [Fact]
        public void Execute_ShouldReturnEmptyList_WhenNoTrackersExist()
        {
            // Arrange
            InMemoryTrackerRepository repository = new();
            GetAllTrackersUseCase useCase = new(repository);

            // Act
            var result = useCase.Execute();

            // Assert
            Assert.Empty(result);
        }
    }
}
